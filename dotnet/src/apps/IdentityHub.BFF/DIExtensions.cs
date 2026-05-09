using System.Reflection;
using IdentityHub.BFF.Clients.Auth;
using IdentityHub.BFF.Clients.File;
using IdentityHub.BFF.Clients.Identity;
using IdentityHub.BFF.Features.Auth.SRPChallenge;
using IdentityHub.BFF.Features.Auth.SRPVerify;
using IdentityHub.BFF.Features.Profile.Avatar;
using IdentityHub.BFF.Features.Profile.GetGeneralInfo;
using IdentityHub.BFF.Features.Profile.Update.UpdateUserName;
using IdentityHub.BFF.Features.PublicKey;
using IdentityHub.BFF.Features.Registration;
using IdentityHub.BFF.Features.User.GenerateCode;
using IdentityHub.BFF.Features.User.RecoveryAccessPassword;
using IdentityHub.BFF.Features.User.VerifyConfirmCode;
using IdentityHub.BFF.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace IdentityHub.BFF
{
    public static class DIExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddMediatR(prop => prop.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddSingleton<JwtReader>();

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.Cookie.Name = "IdentityHub";
                        options.Cookie.Domain = null;
                        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; 
                        options.Cookie.SameSite = SameSiteMode.Lax;
                        options.Cookie.HttpOnly = true;
                        options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    });
            }
            else
            {
                builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.Cookie.Name = "IdentityHub";
                        options.Cookie.Domain = ".terminex.ru"; 
                        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                        options.Cookie.SameSite = SameSiteMode.Strict;
                        options.Cookie.HttpOnly = true;
                        options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    });
            }

            // services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            // {
            //     options.Cookie.Name = "IdentityHub";
            //     options.Cookie.HttpOnly = true;
            //     options.Cookie.SameSite = SameSiteMode.Strict;
            //     options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            // });

            services.AddCors(options =>
            {
                options.AddPolicy
                    (
                        name: "AllowSpecificOrigin",
                            policy =>
                            {
                                policy.WithOrigins("http://127.0.0.1:4200") // Разрешенный домен вашего фронтенда
                                      .WithOrigins("http://localhost:5012")
                                      .WithOrigins("http://localhost:5005")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowCredentials(); // Важно, если вы используете credentials: 'include' на клиенте
                            }
                    );
            });

            return services;
        }

        public static IServiceCollection AddHttpServices(this IServiceCollection services, IConfiguration configuration)
        {
            var authServiceHttp = configuration["AuthService"];

            services.AddHttpClient<IAuthService, AuthService>(client => client.BaseAddress = new Uri(authServiceHttp!));
            
            var identityServiceHttp = configuration["IdentityService"];

            services.AddHttpClient<IIdentityService, IdentityService>(client => client.BaseAddress = new Uri(identityServiceHttp!));

            var minervaServiceHttp = configuration["MinervaService"];

            services.AddHttpClient<IFileService, FileService>(client => client.BaseAddress = new Uri(minervaServiceHttp!));

            return services;
        }

        public static WebApplication AddEndpoints(this WebApplication webApplication)
        {
            webApplication.MapAvatar();
            webApplication.MapGetGeneralInfo();
            webApplication.MapUpdateUserName();
            webApplication.MapSRPChallenge();
            webApplication.MapSRPVerify();
            webApplication.MapRegistration();
            webApplication.MapPublicKey();
            webApplication.MapGenerateCode();
            webApplication.MapVerifyConfirmCode();
            webApplication.MapRecoveryAccessPassword();

            return webApplication;
        }
    }
}