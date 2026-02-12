using Microsoft.Extensions.Configuration;
using Shared.Security.Abstraction.Encoders;
using System.Security.Cryptography;

namespace IdentityHub.AuthService.Application.Services.Protections
{
    public sealed class RSADecryptor(IConfiguration configuration) : IVerifierProtector
    {
        private readonly string _privateKey = configuration["Security:RSA:PrivateKey"] ?? throw new InvalidOperationException("Приватный ключ не сконфигурирован.");

        public string Protect(string verifier)
        {
            throw new NotSupportedException("Данный сервис не обладает функционалом шифрования!");
        }

        public string Unprotect(string protectedVerifier)
        {
            using var rsa = RSA.Create();

            rsa.ImportRSAPrivateKey(Convert.FromBase64String(_privateKey), out _);

            var encryptedData = Convert.FromBase64String(protectedVerifier);
            var decryptedData = rsa.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA256);

            return Convert.ToBase64String(decryptedData);
        }
    }
}
