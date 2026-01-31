using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Domain.ValueObjects.AuthMethod
{
    public readonly record struct AuthData
    {
        public string Value { get; }

        private AuthData(string value) => Value = value;

        public static Result<AuthData> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result<AuthData>.Failure(Error.Validation("Данные для аунтентификации не должны быть пустыми!"));

            return Result<AuthData>.Success(new AuthData(value));
        }

        public static AuthData? Null => null;

        public override string ToString() => Value;
        public static implicit operator string(AuthData login) => login.Value;
    }
}
