using MediatR;
using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Application.Features.Users.GenerateConfirmCode
{
    public sealed record GenerateConfirmCodeCommand(string Login) : IRequest<Result>;
}