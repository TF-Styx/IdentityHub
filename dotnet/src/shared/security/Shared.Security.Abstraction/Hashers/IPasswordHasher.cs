namespace Shared.Security.Abstraction.Hashers
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string storageHash);
        CryptoParameter GetCryptoParameters(string storageHash);
    }
}
