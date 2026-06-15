using IdentityHub.BFF.Clients.Auth;
using IdentityHub.BFF.Clients.File;
using IdentityHub.BFF.Clients.Identity;

namespace IdentityHub.BFF.Extensions.ServiceCollections
{
    public static class HttpClientsExtension
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            var authServiceHttp = configuration["AuthService"];
            services.AddHttpClient<IAuthService, AuthService>(client => client.BaseAddress = new Uri(authServiceHttp!));

            var identityServiceHttp = configuration["IdentityService"];
            services.AddHttpClient<IIdentityService, IdentityService>(client => client.BaseAddress = new Uri(identityServiceHttp!));

            var minervaServiceHttp = configuration["MinervaService"];
            services.AddHttpClient<IFileService, FileService>(client => client.BaseAddress = new Uri(minervaServiceHttp!));

            return services;
        }
    }
}