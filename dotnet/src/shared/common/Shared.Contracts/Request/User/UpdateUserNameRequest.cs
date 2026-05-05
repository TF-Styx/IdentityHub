namespace Shared.Contracts.Request.User
{
    public sealed record UpdateUserNameRequest(string UserId, string UserName);
}