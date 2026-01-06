using Microsoft.Extensions.DependencyInjection;
using Shared.Security.Hashers;
using System.Reflection;

namespace IdentityHub.IdentityService.Application.Ioc
{
    public static class ApplicationDi
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddScoped<IPasswordHasher, Argon2Hasher>();

            return services;
        }
    }
}
