using Shared.Contracts.Request.User;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Clients.Identity
{
    public interface IIdentityService
    {
        Task<Result> RegistrationAsync(RegisterUserRequest request);
        Task<Result> GenerateCodeAsync(string login);
        Task<Result> VerifyConfirmCodeAsync(string login, int code);
        Task<Result> RecoveryAccessPasswordAsync(RecoveryAccessPasswordRequest request);
    }
}