using Shared.Contracts.Response.User;

namespace IdentityHub.AuthService.Application.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateRefreshToken();
        string GenerateAccessToken(UserResponse response);
    }
}
