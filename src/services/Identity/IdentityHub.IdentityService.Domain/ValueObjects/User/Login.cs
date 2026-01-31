using Shared.Kernel.Results;
using System.Text.RegularExpressions;

namespace IdentityHub.IdentityService.Domain.ValueObjects.User
{
    public readonly record struct Login
    {
        public const int MAX_LENGTH = 50;
        public const int MIN_LENGTH = 2;

        public string Value { get; }

        private static readonly Regex AllowedCharsRegex = new("^[a-zA-Z0-9_]+$");

        private Login(string value) { Value = value; }

        public static Result<Login> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result<Login>.Failure(Error.Validation("Логин было пустым!"));

            if (value.Length >= MAX_LENGTH)
                return Result<Login>.Failure(Error.Validation($"Максимально допустимая длина '{nameof(Login)}' = {MAX_LENGTH}!"));

            if (value.Length <= MIN_LENGTH)
                return Result<Login>.Failure(Error.Validation($"Минимально допустимая длина '{nameof(Login)}' = {MIN_LENGTH}!"));

            if (!AllowedCharsRegex.IsMatch(value))
                return Result<Login>.Failure(Error.Validation($"'{nameof(Login)}' содержит недопустимые символы!"));


            return Result<Login>.Success(new Login(value));
        }

        public override string ToString() => Value;
        public static implicit operator string(Login login) => login.Value;
    }
}
