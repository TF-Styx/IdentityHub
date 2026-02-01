using MediatR;
using Shared.Contracts.Response.Auth;
using Shared.Kernel.Results;

namespace IdentityHub.AuthService.Application.Features.VerifySRP
{
    public sealed record VerifySRPCommand(string Login, string A, string M1) : IRequest<Result<AuthResponse>>;
}
