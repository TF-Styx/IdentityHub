using MediatR;
using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Application.Features.Roles.Create
{
    public sealed record CreateRoleCommand(string Name) : IRequest<Result>;
}
