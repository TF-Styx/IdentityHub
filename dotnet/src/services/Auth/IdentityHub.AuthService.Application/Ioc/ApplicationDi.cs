using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using IdentityHub.AuthService.Application.Services.Protections;
using IdentityHub.AuthService.Application.Services;
using Shared.Security.Abstraction.Encoders;
using Shared.Security.Abstraction.Hashers;
using Shared.Security.Cryptography.Hashers;

namespace IdentityHub.AuthService.Application.Ioc
{
    public static class ApplicationDi
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddScoped<IPasswordHasher, Argon2Hasher>();

            services.AddSingleton<IVerifierProtector, RSADecryptor>();

            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}
