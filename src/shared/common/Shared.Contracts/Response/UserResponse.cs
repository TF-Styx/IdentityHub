namespace Shared.Contracts.Response
{
    public sealed record UserResponse(Guid Id, string Login, string UserName, string HashPassword, string Mail);
}
