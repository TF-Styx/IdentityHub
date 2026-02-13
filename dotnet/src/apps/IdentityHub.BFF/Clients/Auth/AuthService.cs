using System.Text.Json;
using Shared.Contracts.Request.SRP;
using Shared.Contracts.Response.Auth;
using Shared.Contracts.Response.SRP;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Clients.Auth
{
    public class AuthService(HttpClient httpClient) : IAuthService
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public async Task<Result<SRPChallengeResponse>> SRPChallenge(SRPChallengeRequest request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/srp/challenge", request, _jsonSerializerOptions);

            if (!response.IsSuccessStatusCode)
                return Result<SRPChallengeResponse>.Failure(Error.New(ErrorCode.Create, await response.Content.ReadAsStringAsync()));

            var dataResult = await response.Content.ReadFromJsonAsync<SRPChallengeResponse>();

            return Result<SRPChallengeResponse>.Success(dataResult!);
        }

        public async Task<Result<AuthResponse>> SRPVerify(SRPVerifyRequest request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/srp/verify", request, _jsonSerializerOptions);

            if (!response.IsSuccessStatusCode)
                return Result<AuthResponse>.Failure(Error.New(ErrorCode.Create, await response.Content.ReadAsStringAsync()));

            var dataResult = await response.Content.ReadFromJsonAsync<AuthResponse>();

            return Result<AuthResponse>.Success(dataResult!);
        }

        public async Task<Result<PublicKeyResponse>> GetCryptoConfig()
        {
            var response = await httpClient.GetAsync("api/configure/public-key");

            if (!response.IsSuccessStatusCode)
                return Result<PublicKeyResponse>.Failure(Error.New(ErrorCode.Create, await response.Content.ReadAsStringAsync()));

            var dataResult = await response.Content.ReadFromJsonAsync<PublicKeyResponse>();

            return Result<PublicKeyResponse>.Success(dataResult!);
        }
    }
}