using MediatR;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Features.Profile.Update.UpdateUserName
{
    public sealed record UpdateUserNameCommand(string UserId, string UserName) : IRequest<Result>;
}