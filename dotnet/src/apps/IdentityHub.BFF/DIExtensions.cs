using System.Reflection;
using IdentityHub.BFF.Clients.Auth;
using IdentityHub.BFF.Clients.Identity;
using IdentityHub.BFF.Features.Auth.SRPChallenge;
using IdentityHub.BFF.Features.Auth.SRPVerify;
using IdentityHub.BFF.Features.PublicKey;
using IdentityHub.BFF.Features.Registration;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace IdentityHub.BFF
{
    public static class DIExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddMediatR(prop => prop.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Cookie.Name = "IdentityHub";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            services.AddCors(options =>
            {
                options.AddPolicy
                    (
                        name: "AllowSpecificOrigin",
                            policy =>
                            {
                                policy.WithOrigins("http://localhost:5173") // Разрешенный домен вашего фронтенда
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

            return services;
        }

        public static WebApplication AddEndpoints(this WebApplication webApplication)
        {
            webApplication.MapSRPChallenge();
            webApplication.MapSRPVerify();
            webApplication.MapRegistration();
            webApplication.MapPublicKey();

            return webApplication;
        }
    }
}