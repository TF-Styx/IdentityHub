using IdentityHub.IdentityService.Domain.ValueObjects.Role;

namespace IdentityHub.IdentityService.Domain.Enums
{
    public sealed class SmartRole
    {
        public RoleId Id { get; }
        public string Name { get; }

        private SmartRole(RoleId id, string name)
        {
            Id = id;
            Name = name;
        }

        public static readonly SmartRole Admin = new(RoleId.Create(Guid.Parse("3cbf2753-5264-4758-861c-a6dd9701a5e6")).Value, "Admin");
        public static readonly SmartRole User = new(RoleId.Create(Guid.Parse("7a53c60f-c710-472e-9e0f-0c90ec41e3a1")).Value, "User");
    }
}
