using IdentityHub.AuthService.Application.Abstracts;
using IdentityHub.AuthService.Infrastructure.Persistence.Contexts;
using IdentityHub.AuthService.Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace IdentityHub.AuthService.Infrastructure.Ioc
{
    public static class InfrastructureDi
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<AuthContext>());

            services.Configure<RedisOptions>(options => configuration.GetSection(RedisOptions.SectionName));
            services.AddSingleton<IConnectionMultiplexer>(provider =>
            {
                string configurationString = configuration["Redis:ConnectionString"]!;

                var config = ConfigurationOptions.Parse(configurationString);

                config.AbortOnConnectFail = false;
                config.ConnectRetry = 3;
                config.ConnectTimeout = 5000;

                return ConnectionMultiplexer.Connect(config);
            });

            return services;
        }
    }
}
