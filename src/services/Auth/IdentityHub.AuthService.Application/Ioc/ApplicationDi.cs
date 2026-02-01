using System.Reflection;
using Shared.Security.Hashers;
using Microsoft.Extensions.DependencyInjection;
using IdentityHub.AuthService.Application.Services.Protections;
using Shared.Security.Encoders;

namespace IdentityHub.AuthService.Application.Ioc
{
    public static class ApplicationDi
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddScoped<IPasswordHasher, Argon2Hasher>();

            services.AddSingleton<IVerifierProtector, RSADecryptor>();

            return services;
        }
    }
}
