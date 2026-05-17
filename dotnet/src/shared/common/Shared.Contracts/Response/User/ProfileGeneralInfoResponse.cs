namespace Shared.Contracts.Response.User
{
    public sealed record ProfileGeneralInfoResponse(string Login, string UserName, string Email, string? Avatar);
}