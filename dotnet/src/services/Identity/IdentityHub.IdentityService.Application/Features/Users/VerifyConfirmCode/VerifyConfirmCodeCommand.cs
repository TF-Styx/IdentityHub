using MediatR;
using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Application.Features.Users.VerifyConfirmCode
{
    public sealed record VerifyConfirmCodeCommand(string Login, int Code) : IRequest<Result>;
}