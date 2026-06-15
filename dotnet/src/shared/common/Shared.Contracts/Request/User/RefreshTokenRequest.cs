namespace Shared.Contracts.Request.User
{
    public sealed record RefreshTokenRequest(string RefreshToken, string? AccessToken = null);
}