namespace Shared.Contracts.Response.User
{
    public sealed record UserResponse(Guid Id, string Login, string UserName, string Password, string ClientSalt, string EncryptedDek, string Email);
}
