namespace Shared.Contracts.Response.Auth
{
    public sealed record CompleteSrpAuthResponse(string SessionId, string UserId, string Login, string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAt);
}