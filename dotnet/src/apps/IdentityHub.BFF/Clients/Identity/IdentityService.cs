using System.Text;
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

        public async Task<Result> GenerateCodeAsync(string login)
        {
            var response = await httpClient.PostAsync($"api/users/generate-recovery-access-code/{login}",  new StringContent("", Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                return Result.Failure(Error.New(ErrorCode.Create, await response.Content.ReadAsStringAsync()));

            return Result.Success();
        }

        public async Task<Result> VerifyConfirmCodeAsync(string login, int code)
        {
            var response = await httpClient.PostAsync($"api/users/verify-confirm-code/{login}/{code}",  new StringContent("", Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                return Result.Failure(Error.New(ErrorCode.Create, await response.Content.ReadAsStringAsync()));

            return Result.Success();
        }

        public async Task<Result> RecoveryAccessPasswordAsync(RecoveryAccessPasswordRequest request)
        {
            var response = await httpClient.PatchAsJsonAsync($"api/users/recovery-access-password", request, _jsonSerializerOptions);

            if (!response.IsSuccessStatusCode)
                return Result.Failure(Error.New(ErrorCode.Create, await response.Content.ReadAsStringAsync()));

            return Result.Success();
        }
    }
}