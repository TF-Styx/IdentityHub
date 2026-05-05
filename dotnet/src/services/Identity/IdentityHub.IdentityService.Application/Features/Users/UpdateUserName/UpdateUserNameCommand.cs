using MediatR;
using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Application.Features.Users.UpdateUserName
{
    public sealed record UpdateUserNameCommand(Guid UserId, string UserName) : IRequest<Result>;
}