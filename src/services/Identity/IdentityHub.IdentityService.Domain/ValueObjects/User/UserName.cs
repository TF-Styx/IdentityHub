using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Domain.ValueObjects.User
{
    public readonly record struct UserName
    {
        public const int MAX_LENGTH = 100;
        public const int MIN_LENGTH = 2;

        public string Value { get; }

        private UserName(string value) => Value = value; 

        public static Result<UserName> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result<UserName>.Failure(Error.Validation("Логин было пустым!"));

            if (value.Length >= MAX_LENGTH)
                return Result<UserName>.Failure(Error.Validation($"Максимально допустимая длина '{nameof(UserName)}' = {MAX_LENGTH}!"));

            if (value.Length <= MIN_LENGTH)
                return Result<UserName>.Failure(Error.Validation($"Минимально допустимая длина '{nameof(UserName)}' = {MIN_LENGTH}!"));

            var trimmedUserName = value.Trim();

            return Result<UserName>.Success(new UserName(trimmedUserName));
        }

        public override string ToString() => Value;
        public static implicit operator string(UserName name) => name.Value;
    }
}
