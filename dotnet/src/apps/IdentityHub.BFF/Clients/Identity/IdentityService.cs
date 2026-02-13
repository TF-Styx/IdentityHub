using System.Text.Json;
using Shared.Contracts.Request.User;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Clients.Identity
{
    public class IdentityService(HttpClient httpClient) : IIdentityService
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public async Task<Result> RegistrationAsync(RegisterUserRequest request)
        {
            var response = await httpClient.PostAsJsonAsync("api/users", request, _jsonSerializerOptions);

            if (!response.IsSuccessStatusCode)
                return Result.Failure(Error.New(ErrorCode.Create, await response.Content.ReadAsStringAsync()));

            return Result.Success();
        }
    }
}