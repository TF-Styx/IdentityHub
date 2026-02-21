using IdentityHub.IdentityService.Domain.ValueObjects.Role;
using IdentityHub.IdentityService.Domain.ValueObjects.User;
using Shared.Kernel.Primitives;

namespace IdentityHub.IdentityService.Domain.Models
{
    public sealed class UserRoles : Entity<Guid>
    {
        public UserId UserId { get; private set; }
        public RoleId RoleId { get; private set; }

        private UserRoles() { }
        private UserRoles(Guid id, UserId userId, RoleId roleId) : base(id)
        {
            UserId = userId;
            RoleId = roleId;
        }

        internal static UserRoles Create(UserId userId, RoleId roleId)
            => new UserRoles(Guid.NewGuid(), userId, roleId);
    }
}
