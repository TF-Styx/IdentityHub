using MediatR;
using Shared.Kernel.Results;
using Shared.Contracts.Response.User;

namespace IdentityHub.IdentityService.Application.Features.Users.InternalUser.InternalUserById
{
    public sealed record InternalUserByIdQuery(Guid UserId) : IRequest<Result<UserResponse>>;
}