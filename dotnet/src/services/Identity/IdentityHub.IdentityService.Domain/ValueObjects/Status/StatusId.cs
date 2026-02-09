using IdentityHub.IdentityService.Domain.ValueObjects.User;
using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Domain.ValueObjects.Status
{
    public readonly record struct StatusId
    {
        public Guid Value { get; }

        internal StatusId(Guid value) { Value = value; }

        public static StatusId New() => new(Guid.NewGuid());

        public static Result<StatusId> Create(Guid value)
        {
            if (value == Guid.Empty)
                return Result<StatusId>.Failure(Error.New(ErrorCode.ValueObject, "Не удалось создать VO для UserId! Был передан пустой Guid!"));

            return Result<StatusId>.Success(new StatusId(value));
        }

        public override string ToString() => Value.ToString();
        public static implicit operator Guid(StatusId idRole) => idRole.Value;
    }
}
