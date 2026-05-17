namespace Shared.Contracts.Response.BFF
{
    public sealed record ProfileGeneralInfoBFFResponse(string Login, string UserName, string Email, string? Avatar);
}