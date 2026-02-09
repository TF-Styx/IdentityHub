using MediatR;
using Shared.Kernel.Results;
using Shared.Contracts.Response.User;

namespace IdentityHub.IdentityService.Application.Features.Users.InternalUser.InternalUserByLogin
{
    public sealed record InternalUserByLoginQuery(string Login) : IRequest<Result<UserResponse>>;
}