using MediatR;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Features.User.GenerateCode
{
    public sealed record GenerateCodeCommand(string Login) : IRequest<Result>;
}