using Shared.Redis;
using System.Reflection;
using StackExchange.Redis;
using IdentityHub.BFF.Services;

namespace IdentityHub.BFF.Extensions.ServiceCollections
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(prop => prop.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddSingleton<JwtReader>();

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
            services.AddSingleton<RedisService>();

            return services;
        }
    }
}