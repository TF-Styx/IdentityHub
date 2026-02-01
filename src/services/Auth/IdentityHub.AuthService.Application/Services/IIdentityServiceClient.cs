using Shared.Contracts.Response.User;
using Shared.Kernel.Primitives;

namespace IdentityHub.AuthService.Application.Services
{
    public interface IIdentityServiceClient
    {
        Task<Maybe<UserResponse>> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<Maybe<UserResponse>> GetUserByLoginAsync(string userId, CancellationToken cancellationToken = default);
    }
}
