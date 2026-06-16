using StackExchange.Redis;
using Shared.Contracts.Common;
using IdentityHub.BFF.Services;
using Medallion.Threading.Redis;
using Shared.Contracts.CacheKeys;
using IdentityHub.BFF.Clients.Auth;
using Shared.Contracts.Request.User;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Medallion.Threading;

namespace IdentityHub.BFF.Extensions.ServiceCollections
{
    public static class AuthenticationService
    {
        public static IServiceCollection UseCookie(this IServiceCollection services, IWebHostEnvironment environment)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "Terminex";
                    options.ExpireTimeSpan = TimeSpan.FromDays(30);
                    options.Cookie.HttpOnly = true;
                    options.SlidingExpiration = true; // Динамическое продление времени жизни

                    if (environment.IsDevelopment())
                    {
                        options.Cookie.Domain = "127.0.0.1";
                        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                        options.Cookie.SameSite = SameSiteMode.Lax;
                    }
                    else
                    {
                        options.Cookie.Domain = ".terminex.ru";
                        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                        options.Cookie.SameSite = SameSiteMode.Strict;
                    }

                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    };

                    options.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = 403;
                        return Task.CompletedTask;
                    };

                    options.Events.OnValidatePrincipal = async context =>
                    {
                        var sessionId = context.Principal?.FindFirst("SessionId")?.Value;

                        if (string.IsNullOrWhiteSpace(sessionId))
                        {
                            // Используется для отмены/аннулирования текущей аутентификации пользователя при проверке подлинности на основе cookie
                            context.RejectPrincipal();
                            return;
                        }

                        var sessionKey = RedisKeys.SessionString(sessionId);
                        var redisService = context.HttpContext.RequestServices.GetRequiredService<RedisService>();
                        var userSessionResult = await redisService.GetJsonAsync<UserSession>(sessionKey);
                        
                        if (userSessionResult.IsFailure)
                        {
                            // Используется для отмены/аннулирования текущей аутентификации пользователя при проверке подлинности на основе cookie
                            context.RejectPrincipal();
                            return;
                        }

                        var userSession = userSessionResult.Value;

                        var oneMinute = DateTime.UtcNow.AddMinutes(1);
                        
                        if (userSession.AccessTokenExpiresAt <= oneMinute)
                        {
                            var lockProvider = context.HttpContext.RequestServices.GetRequiredService<RedisDistributedSynchronizationProvider>();

                            var lockKey = RedisKeys.LockKeyString(sessionId);

                            await using var handle = await lockProvider.TryAcquireLockAsync(lockKey, TimeSpan.FromSeconds(5));

                            if (handle == null)
                            {
                                // Используется для отмены/аннулирования текущей аутентификации пользователя при проверке подлинности на основе cookie
                                context.RejectPrincipal();
                                return;
                            }

                            var reGetUserSessionResult = await redisService.GetJsonAsync<UserSession>(sessionKey);

                            if (reGetUserSessionResult.IsFailure)
                            {
                                // Используется для отмены/аннулирования текущей аутентификации пользователя при проверке подлинности на основе cookie
                                context.RejectPrincipal();
                                return;
                            }

                            userSession = reGetUserSessionResult.Value;

                            if (userSession.AccessTokenExpiresAt <= oneMinute)
                            {
                                var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();

                                var authResponseResult = await authService.RefreshToken(new RefreshTokenRequest(userSession.RefreshToken, userSession.AccessToken));

                                if (authResponseResult.IsSuccess)
                                {
                                    var authResponse = authResponseResult.Value;

                                    var jwtReader = context.HttpContext.RequestServices.GetRequiredService<JwtReader>();

                                    var extract = jwtReader.Extract(authResponse.AccessToken);

                                    userSession.AccessToken = authResponse.AccessToken;
                                    userSession.RefreshToken = authResponse.RefreshToken;
                                    userSession.AccessTokenExpiresAt = extract.ExpiredTime;
                                    
                                    await redisService.SetJsonAsync(sessionKey, userSession, TimeSpan.FromDays(30));
                                }
                                else
                                {
                                    await redisService.DeleteAsync(sessionKey);
                                    context.RejectPrincipal();
                                    return;
                                }
                            }
                        }

                        context.HttpContext.Items["AccessToken"] = userSession.AccessToken;
                    };
                });

            return services;
        }

        public static IServiceCollection AddDistributedLock(this IServiceCollection services)
        {
            services.AddSingleton(serviceProvider =>
            {
                var redis = serviceProvider.GetRequiredService<IConnectionMultiplexer>();

                return new RedisDistributedSynchronizationProvider(redis.GetDatabase());
            });

            return services;
        }

        public static IServiceCollection AddSharedCryptoKeyASPNET(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration["Redis:ConnectionString"]!;
            var redis = ConnectionMultiplexer.Connect(connectionString);

            services.AddDataProtection().SetApplicationName("Terminex.SharedBff").PersistKeysToStackExchangeRedis(redis, RedisKeys.DataProtectionString());

            return services;
        }

        public static IServiceCollection UseCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy
                    (
                        name: "AllowSpecificOrigin",
                            policy =>
                            {
                                policy.WithOrigins("http://127.0.0.1:4200", "http://localhost:5012", "http://localhost:5005")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowCredentials();
                            }
                    );
            });

            return services;
        }
    }
}