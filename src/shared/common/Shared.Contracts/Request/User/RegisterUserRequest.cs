namespace Shared.Contracts.Request.User
{
    public sealed record RegisterUserRequest(string Login, string UserName, string Password, string ClientSAlt, string EncryptedDek, string Mail, List<string> RoleIds);
}
