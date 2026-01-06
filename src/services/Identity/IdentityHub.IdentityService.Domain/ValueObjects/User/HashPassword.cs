using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Domain.ValueObjects.User
{
    public readonly record struct HashPassword
    {
        public string Value { get; }

        private HashPassword(string value) => Value = value;

        public static Result<HashPassword> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result<HashPassword>.Failure(Error.Validation("Вы не ввели пароль!"));

            return Result<HashPassword>.Success(new HashPassword(value));
        }

        public override string ToString() => Value.ToString();
        public static implicit operator string(HashPassword hashPassword) => hashPassword.Value;
    }
}
