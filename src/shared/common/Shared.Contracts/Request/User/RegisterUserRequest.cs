namespace Shared.Contracts.Request.User
{
    public sealed record RegisterUserRequest
        (
            string Login,
            string UserName,
            string Verifier,
            string ClientSalt,
            string EncryptedDek,
            string EncryptionAlgorithm,
            int Iterations,
            string KdfType,
            string Email, string? Phone
        );
}
