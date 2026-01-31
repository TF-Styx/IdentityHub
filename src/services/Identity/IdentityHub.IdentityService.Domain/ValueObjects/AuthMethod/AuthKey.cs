using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Domain.ValueObjects.AuthMethod
{
    public readonly record struct AuthKey
    {
        public string Value { get; }

        private AuthKey(string value) => Value = value;

        public static Result<AuthKey> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result<AuthKey>.Failure(Error.Validation("Ключ аунтентификации не должен быть пустым!"));

            return Result<AuthKey>.Success(new AuthKey(value));
        }

        public override string ToString() => Value;
        public static implicit operator string(AuthKey login) => login.Value;
    }
}
