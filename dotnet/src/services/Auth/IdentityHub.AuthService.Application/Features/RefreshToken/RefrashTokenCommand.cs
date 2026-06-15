using MediatR;
using Shared.Contracts.Response.Auth;
using Shared.Kernel.Results;

namespace IdentityHub.AuthService.Application.Features.RefreshToken
{
    public sealed record RefreshTokenCommand(string RefreshToken, string? AccessToken = null) : IRequest<Result<AuthResponse>>;
}