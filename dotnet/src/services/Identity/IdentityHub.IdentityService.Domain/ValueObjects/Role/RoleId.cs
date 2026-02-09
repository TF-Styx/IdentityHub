using IdentityHub.IdentityService.Domain.ValueObjects.Status;
using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Domain.ValueObjects.Role
{
    public readonly record struct RoleId
    {
        public Guid Value { get; }

        internal RoleId(Guid value) => Value = value;

        public static RoleId New() => new(Guid.NewGuid());

        public static Result<RoleId> Create(Guid value)
        {
            if (value == Guid.Empty)
                return Result<RoleId>.Failure(Error.New(ErrorCode.ValueObject, "Не удалось создать VO для UserId! Был передан пустой Guid!"));

            return Result<RoleId>.Success(new RoleId(value));
        }

        public override string ToString() => Value.ToString();
        public static implicit operator Guid(RoleId idRole) => idRole.Value;
    }
}
