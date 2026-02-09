using IdentityHub.IdentityService.Domain.ValueObjects.Role;
using Shared.Kernel.Primitives;
using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Domain.Models
{
    public sealed class Role : AggregateRoot<RoleId>
    {
        public RoleName Name { get; private set; }

        private Role() { }
        private Role(RoleId id, RoleName roleName) : base(id)
            => Name = roleName;

        public static Result<Role> Create(RoleId id, RoleName roleName)
        {
            var roleIdResult = RoleId.Create(id);
            var roleNameResult = RoleName.Create(roleName);

            if (roleIdResult.IsFailure || roleNameResult.IsFailure)
            {
                var errors = new List<Error>();

                errors.AddRange(roleIdResult.Errors);
                errors.AddRange(roleNameResult.Errors);

                return Result<Role>.Failure(errors);
            }

            var role = new Role
                (
                    roleIdResult.Value,
                    roleNameResult.Value
                );

            return Result<Role>.Success(role);
        }

        public void UpdateRoleName(RoleName roleName)
            => Name = roleName;
    }
}
