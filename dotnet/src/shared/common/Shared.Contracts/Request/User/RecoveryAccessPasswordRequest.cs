namespace Shared.Contracts.Request.User
{
    public sealed record RecoveryAccessPasswordRequest
        (
            string Login,
            string Verifier,
            string ClientSalt,
            string EncryptedDek,
            string EncryptionAlgorithm,
            int Iterations,
            string KdfType
        );
}