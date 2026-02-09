using Shared.Kernel.Results;
using System.Text.Json;

namespace IdentityHub.IdentityService.Domain.ValueObjects.SecureData
{
    public readonly record struct EncryptedMetadata
    {
        public const int MIN_COUN_ITERATIONS = 100000;

        public string Algorithm { get; }
        public int Iterations { get; }
        public string KdfType { get; }

        internal EncryptedMetadata(string algorithm, int iterations, string kdfType)
        {
            Algorithm = algorithm;
            Iterations = iterations;
            KdfType = kdfType;
        }

        public static Result<EncryptedMetadata> Create(string algorithm, int iterations, string kdfType)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(algorithm))
                errors.Add(Error.Validation("Алгоритм шифрования не должен быть пустым!"));

            if (iterations < MIN_COUN_ITERATIONS)
                errors.Add(Error.Validation("Количество итераций должно быть больше минимального кол-во итераций!"));

            if (errors.Count > 0)
                return Result<EncryptedMetadata>.Failure(errors);

            return Result<EncryptedMetadata>.Success(new EncryptedMetadata(algorithm, iterations, kdfType));
        }

        public static EncryptedMetadata FromStorage(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default;

            return JsonSerializer.Deserialize<EncryptedMetadata>(json);
        }

        public override string ToString() => JsonSerializer.Serialize(this);
        public static implicit operator string(EncryptedMetadata metadata) => metadata.ToString();
    }
}
