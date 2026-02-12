namespace Shared.Contracts.Response.Auth
{
    public sealed record AuthResponse(string AccessToken, string RefreshToken, string? M2 = null);
}
