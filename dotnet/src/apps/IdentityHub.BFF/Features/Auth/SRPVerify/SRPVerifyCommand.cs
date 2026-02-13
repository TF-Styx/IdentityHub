using MediatR;
using Shared.Contracts.Response.Auth;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Features.Auth.SRPVerify
{
    public sealed record SRPVerifyCommand(string Login, string A, string M1) : IRequest<Result<AuthResponse>>;
}