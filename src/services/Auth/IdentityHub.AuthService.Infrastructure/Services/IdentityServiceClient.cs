using IdentityHub.AuthService.Application.Services;
using Shared.Contracts.Response.User;
using Shared.Kernel.Primitives;
using System.Net.Http.Json;

namespace IdentityHub.AuthService.Infrastructure.Services
{
    public sealed class IdentityServiceClient(HttpClient httpClient) : IIdentityServiceClient
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<Maybe<UserResponse>> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<UserResponse>($"internal/api/users/by-id/{userId}", cancellationToken);

                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Maybe<UserResponse>> GetUserByLoginAsync(string userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<UserResponse>($"internal/api/users/y-login/{userId}", cancellationToken);

                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
