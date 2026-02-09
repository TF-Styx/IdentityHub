using Shared.Kernel.Results;
using System.Numerics;
using System.Text.RegularExpressions;

namespace IdentityHub.IdentityService.Domain.ValueObjects.User
{
    public readonly record struct NumberPhone
    {
        public string Value { get; }

        internal NumberPhone(string value) => Value = value;

        private static readonly Regex PhoneRegex = new(@"^+?[0-9\s-]{7,20}$");

        /// <exception cref="PhoneNumberException"></exception>
        public static Result<NumberPhone> Create(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return Result<NumberPhone>.Failure(Error.ValidationPhone());

            var sanitizedPhone = phoneNumber.Trim();

            if(!PhoneRegex.IsMatch(sanitizedPhone))
                return Result<NumberPhone>.Failure(Error.ValidationPhone());

            return Result<NumberPhone>.Success(new NumberPhone(sanitizedPhone));
        }

        public static NumberPhone? Null => null;

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value))
                return string.Empty;

            return Value;
        }

        public static implicit operator string(NumberPhone phone) => phone.Value;
    }
}
