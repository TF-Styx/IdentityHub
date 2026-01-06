using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Domain.ValueObjects.Status
{
    public readonly record struct StatusName
    {
        public const int MAX_LENGTH = 50;
        public const int MIN_LENGTH = 5;

        public string Value { get; }

        /// <summary>
        /// Конструктор использует EFC
        /// </summary>
        /// <param name="value"></param>
        private StatusName(string value) { Value = value; }

        public static Result<StatusName> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result<StatusName>.Failure(Error.Validation("Наименование роли было пустым!"));

            if (value.Length >= MAX_LENGTH)
                return Result<StatusName>.Failure(Error.Validation($"Максимально допустимая длина '{nameof(StatusName)}' = {MAX_LENGTH}!"));

            if (value.Length <= MIN_LENGTH)
                return Result<StatusName>.Failure(Error.Validation($"Минимально допустимая длина '{nameof(StatusName)}' = {MIN_LENGTH}!"));

            var trimmedUserName = value.Trim();

            return Result<StatusName>.Success(new StatusName(trimmedUserName));
        }

        public override string ToString() => Value.ToString();
        public static implicit operator string(StatusName roleName) => roleName.Value;
    }

}
