using IdentityHub.IdentityService.Domain.Models;
using IdentityHub.IdentityService.Domain.ValueObjects.Role;
using IdentityHub.IdentityService.Domain.ValueObjects.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityHub.IdentityService.Infrastructure.Persistence.Configurations
{
    internal sealed class UserRolesConfiguration : IEntityTypeConfiguration<UserRoles>
    {
        public void Configure(EntityTypeBuilder<UserRoles> builder)
        {
            builder.ToTable("UserRoles");
            builder.HasKey(ur => ur.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .ValueGeneratedNever();

            builder.Property(x => x.UserId)
                   .HasColumnName("UserId")
                   .HasConversion(inDB => inDB.Value, outDB => new UserId(outDB));

            builder.Property(x => x.RoleId)
                   .HasColumnName("RoleId")
                   .HasConversion(inDB => inDB.Value, outDB => new RoleId(outDB));

            builder.HasOne<Role>()
                   .WithMany()
                   .HasForeignKey(ur => ur.RoleId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
