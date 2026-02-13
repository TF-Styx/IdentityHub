using Shared.Contracts.Request.User;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Clients.Identity
{
    public interface IIdentityService
    {
        Task<Result> RegistrationAsync(RegisterUserRequest request);
    }
}