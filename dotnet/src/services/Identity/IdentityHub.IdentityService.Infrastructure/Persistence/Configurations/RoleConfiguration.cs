using IdentityHub.IdentityService.Domain.Models;
using IdentityHub.IdentityService.Domain.ValueObjects.Role;
using IdentityHub.IdentityService.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityHub.IdentityService.Infrastructure.Persistence.Configurations
{
    internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever()
                .HasConversion(roleId => roleId.Value, dbValue => new RoleId(dbValue));

            builder.Property(s => s.Name)
                .HasColumnName("Name")
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .HasConversion(name => name.Value, dbValue => new RoleName(dbValue));
        }
    }
}
