using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Domain.ValueObjects.User
{
    public readonly record struct UserId
    {
        public Guid Value { get; }

        private UserId(Guid value) => Value = value;

        public static UserId New() => new(Guid.NewGuid());

        public static Result<UserId> Create(Guid value)
        {
            if (value == Guid.Empty)
                return Result<UserId>.Failure(Error.New(ErrorCode.ValueObject, "Не удалось создать VO для UserId! Был передан пустой Guid!"));

            return Result<UserId>.Success(new UserId(value));
        }

        public override string ToString() => Value.ToString();
        public static implicit operator Guid(UserId userId) => userId.Value;
    }
}
