using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Domain.ValueObjects.Role
{
    public readonly record struct RoleName
    {
        public const int MAX_LENGTH = 50;
        public const int MIN_LENGTH = 5;

        public string Value { get; }

        /// <summary>
        /// Конструктор использует EFC
        /// </summary>
        /// <param name="value"></param>
        private RoleName(string value) { Value = value; }

        public static Result<RoleName> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result<RoleName>.Failure(Error.Validation("Наименование роли было пустым!"));

            if (value.Length >= MAX_LENGTH)
                return Result<RoleName>.Failure(Error.Validation($"Максимально допустимая длина '{nameof(RoleName)}' = {MAX_LENGTH}!"));

            if (value.Length <= MIN_LENGTH)
                return Result<RoleName>.Failure(Error.Validation($"Минимально допустимая длина '{nameof(RoleName)}' = {MIN_LENGTH}!"));

            var trimmedUserName = value.Trim();

            return Result<RoleName>.Success(new RoleName(trimmedUserName));
        }

        public override string ToString() => Value.ToString();
        public static implicit operator string(RoleName roleName) => roleName.Value;
    }

}
