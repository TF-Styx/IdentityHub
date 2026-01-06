using IdentityHub.IdentityService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityHub.IdentityService.Infrastructure.Ioc
{
    public static class InfrastructureDi
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionStrings = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<IdentityContext>(options => options.UseNpgsql(connectionStrings));

            return services;
        }
    }
}
