using Shared.Kernel.Results;
using Shared.Contracts.Request.SRP;
using Shared.Contracts.Request.User;
using Shared.Contracts.Response.Auth;
using Shared.Contracts.Response.SRP;

namespace IdentityHub.BFF.Clients.Auth
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> RefreshToken(RefreshTokenRequest request);
        Task<Result<SRPChallengeResponse>> SRPChallenge(SRPChallengeRequest request);
        Task<Result<AuthResponse>> SRPVerify(SRPVerifyRequest request);
        Task<Result<string>> GetCryptoConfig();
    }
}