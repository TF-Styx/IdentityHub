using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Domain.ValueObjects.SecureData
{
    public readonly record struct EncryptedValue
    {
        public string Value { get; }

        internal EncryptedValue(string value) => Value = value;

        public static Result<EncryptedValue> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result<EncryptedValue>.Failure(Error.Validation("Зашифрованное значение не должно быть пустым!"));

            return Result<EncryptedValue>.Success(new EncryptedValue(value));
        }

        public override string ToString() => Value;
        public static implicit operator string(EncryptedValue login) => login.Value;
    }
}
