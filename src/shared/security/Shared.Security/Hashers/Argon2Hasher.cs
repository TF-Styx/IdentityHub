using System.Text;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;

namespace Shared.Security.Hashers
{
    public sealed class Argon2Hasher : IPasswordHasher
    {
        private const int DegreeOfParallelism = 2;
        private const int Iterations = 3;
        private const int MemorySizeKB = 65536;
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const string HashIdentifier = "Argon2Id";

        public string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var argon2 = new Argon2id(passwordBytes)
            {
                Salt = salt,
                DegreeOfParallelism = DegreeOfParallelism,
                Iterations = Iterations,
                MemorySize = MemorySizeKB
            };
            var hash = argon2.GetBytes(HashSize);

            string saltBase64 = Convert.ToBase64String(salt);
            string hashBase64 = Convert.ToBase64String(hash);

            return $"Argon2id:{DegreeOfParallelism}:{Iterations}:{MemorySizeKB}:{saltBase64}:{hashBase64}";
        }

        public bool Verify(string password, string storageHash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(storageHash))
                return false;

            var parseResult = ParseHashString(storageHash);

            if (!parseResult.Success)
                return false;

            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var argon2 = new Argon2id(passwordBytes)
            {
                Salt = parseResult.Parameters.Salt,
                DegreeOfParallelism = parseResult.Parameters.DegreeOfParallelism,
                Iterations = parseResult.Parameters.Iterations,
                MemorySize = parseResult.Parameters.MemorySizeKB
            };
            var hash = argon2.GetBytes(parseResult.StorageHash.Length);

            bool verified = CryptographicOperations.FixedTimeEquals(hash, parseResult.StorageHash);

            return verified;
        }

        public CryptoParameter GetCryptoParameters(string storageHash)
        {
            var result = ParseHashString(storageHash);

            return result.Parameters;
        }

        private (bool Success, CryptoParameter Parameters, byte[] StorageHash) ParseHashString(string storageHashString)
        {
            if (string.IsNullOrWhiteSpace(storageHashString))
                return (false, null!, null!);

            string[] parts = storageHashString.Split(':');

            if (parts.Length != 5 || parts[0] != HashIdentifier)
                return (false, null!, null!);

            try
            {
                var parameter = new CryptoParameter(int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3]), Convert.FromBase64String(parts[4]));

                byte[] storageHash = Convert.FromBase64String(parts[5]);

                return (true, parameter, storageHash);
            }
            catch (Exception)
            {
                return (false, null!, null!);
            }
        }
    }
}
