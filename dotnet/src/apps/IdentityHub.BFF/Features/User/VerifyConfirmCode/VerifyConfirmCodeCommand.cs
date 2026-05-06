using MediatR;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Features.User.VerifyConfirmCode
{
    public sealed record VerifyConfirmCodeCommand(string Login, int Code) : IRequest<Result>;
}