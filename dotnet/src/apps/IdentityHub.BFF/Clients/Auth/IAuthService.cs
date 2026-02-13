using Shared.Contracts.Request.SRP;
using Shared.Contracts.Response.Auth;
using Shared.Contracts.Response.SRP;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Clients.Auth
{
    public interface IAuthService
    {
        Task<Result<SRPChallengeResponse>> SRPChallenge(SRPChallengeRequest request);
        Task<Result<AuthResponse>> SRPVerify(SRPVerifyRequest request);
        Task<Result<PublicKeyResponse>> GetCryptoConfig();
    }
}