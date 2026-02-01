using MediatR;
using Shared.Contracts.Response.Auth;
using Shared.Kernel.Results;

namespace IdentityHub.AuthService.Application.Features.LoginByToken
{
    public sealed record LoginByTokenCommand(string RefreshToken) : IRequest<Result<AuthResponse>>;
}
