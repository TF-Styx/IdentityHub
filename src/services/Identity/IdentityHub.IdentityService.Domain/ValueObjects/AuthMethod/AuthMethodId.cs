using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Domain.ValueObjects.AuthMethod
{
    public readonly record struct AuthMethodId
    {
        public Guid Value { get; }

        private AuthMethodId(Guid value) => Value = value;

        public static AuthMethodId New() => new(Guid.NewGuid());

        public static Result<AuthMethodId> Create(Guid value)
        {
            if (value != Guid.Empty)
                return Result<AuthMethodId>.Failure(Error.New(ErrorCode.ValueObject, "Не удалось создать VO для AuthMethodId! Был передан пустой Guid!"));

            return Result<AuthMethodId>.Success(new AuthMethodId(value));
        }

        public override string ToString() => Value.ToString();
        public static implicit operator Guid(AuthMethodId value) => value.Value;
    }
}
