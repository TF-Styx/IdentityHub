using Shared.Kernel.Results;
using System.Text.RegularExpressions;

namespace IdentityHub.IdentityService.Domain.ValueObjects.User
{
    public readonly record struct Email
    {
        public string Value { get; }

        private Email(string value) => Value = value; 

        private static readonly Regex AllowedCharsRegex = new(@"^[^@\s]+@[^@\s]+.[^@\s]+$");

        public static Result<Email> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result<Email>.Failure(Error.Validation("Почта былы пустой!"));

            if (!AllowedCharsRegex.IsMatch(value))
                return Result<Email>.Failure(Error.Validation($"'{nameof(Email)}' содержит недопустимые символы!"));

            return Result<Email>.Success(new Email(value));
        }

        public override string ToString() => Value;
        public static implicit operator string(Email mail) => mail.Value;
    }
}
