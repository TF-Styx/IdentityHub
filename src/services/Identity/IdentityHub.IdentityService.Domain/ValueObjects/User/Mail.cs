using Shared.Kernel.Results;
using System.Text.RegularExpressions;

namespace IdentityHub.IdentityService.Domain.ValueObjects.User
{
    public readonly record struct Mail
    {
        public string Value { get; }

        private Mail(string value) => Value = value; 

        private static readonly Regex AllowedCharsRegex = new(@"^[^@\s]+@[^@\s]+.[^@\s]+$");

        public static Result<Mail> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result<Mail>.Failure(Error.Validation("Почта былы пустой!"));

            if (!AllowedCharsRegex.IsMatch(value))
                return Result<Mail>.Failure(Error.Validation($"'{nameof(Mail)}' содержит недопустимые символы!"));

            return Result<Mail>.Success(new Mail(value));
        }

        public override string ToString() => Value;
        public static implicit operator string(Mail mail) => mail.Value;
    }
}
