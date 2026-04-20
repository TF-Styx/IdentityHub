using IdentityHub.IdentityService.Application.Abstracts;
using IdentityHub.IdentityService.Infrastructure.Persistence.Contexts;
using IdentityHub.IdentityService.Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Redis;
using StackExchange.Redis;

namespace IdentityHub.IdentityService.Infrastructure.Ioc
{
    public static class InfrastructureDi
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionStrings = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<IdentityContext>(options => options.UseNpgsql(connectionStrings));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<IdentityContext>());

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
            services.AddSingleton<IRedisService, RedisService>();

            return services;
        }
    }
}
