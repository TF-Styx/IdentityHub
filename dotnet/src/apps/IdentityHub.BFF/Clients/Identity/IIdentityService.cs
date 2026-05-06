using Shared.Contracts.Request.User;
using Shared.Contracts.Response.User;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Clients.Identity
{
    public interface IIdentityService
    {
        Task<Result<ProfileGeneralInfoResponse?>> GetGeneralInfoAsync(string userId);
        Task<Result> UpdateUserNameAsync(UpdateUserNameRequest request);
        Task<Result> RegistrationAsync(RegisterUserRequest request);
        Task<Result> GenerateCodeAsync(string login);
        Task<Result> VerifyConfirmCodeAsync(string login, int code);
        Task<Result> RecoveryAccessPasswordAsync(RecoveryAccessPasswordRequest request);
    }
}