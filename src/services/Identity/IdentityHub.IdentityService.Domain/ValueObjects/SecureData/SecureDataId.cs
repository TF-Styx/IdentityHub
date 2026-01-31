using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Domain.ValueObjects.SecureData
{
    public readonly record struct SecureDataId
    {
        public Guid Value { get; }

        private SecureDataId(Guid value) => Value = value;

        public static SecureDataId New() => new(Guid.NewGuid());

        public static Result<SecureDataId> Create(Guid value)
        {
            if (value == Guid.Empty)
                return Result<SecureDataId>.Failure(Error.New(ErrorCode.ValueObject, "Не удалось создать VO для SecureDataId! Был передан пустой Guid!"));

            return Result<SecureDataId>.Success(new SecureDataId(value));
        }

        public override string ToString() => Value.ToString();
        public static implicit operator Guid(SecureDataId value) => value.Value;
    }
}
