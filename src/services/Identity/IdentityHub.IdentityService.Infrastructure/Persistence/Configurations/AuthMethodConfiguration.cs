using IdentityHub.IdentityService.Domain.Models;
using IdentityHub.IdentityService.Domain.ValueObjects.AuthMethod;
using IdentityHub.IdentityService.Domain.ValueObjects.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityHub.IdentityService.Infrastructure.Persistence.Configurations
{
    internal sealed class AuthMethodConfiguration : IEntityTypeConfiguration<AuthMethod>
    {
        public void Configure(EntityTypeBuilder<AuthMethod> builder)
        {
            builder.ToTable("AuthMethod");
            builder.HasKey(am => am.Id);

            builder.Property(am => am.Id)
                   .HasColumnName("Id")
                   .ValueGeneratedNever()
                   .HasConversion(outDB => outDB.Value, inDB => AuthMethodId.Create(inDB).Value);

            builder.Property(am => am.UserId)
                   .HasColumnName("UserId")
                   .HasConversion(outDB => outDB.Value, inDB => UserId.Create(inDB).Value);

            builder.Property(am => am.AuthType)
                   .HasColumnName("AuthType")
                   .HasConversion<int>();

            builder.Property(am => am.AuthKey)
                   .HasColumnName("AuthKey")
                   .HasConversion(outDB => outDB.Value, inDB => AuthKey.Create(inDB).Value);

            builder.Property(am => am.AuthData)
                   .HasColumnName("AuthData")
                   .IsRequired(false)
                   .HasConversion(outDB => outDB.HasValue ? outDB.Value.Value : null, inDB => string.IsNullOrWhiteSpace(inDB) ? null : AuthData.Create(inDB).Value);

            builder.Property(am => am.Salt)
                   .HasColumnName("Salt")
                   .HasMaxLength(256)
                   .IsRequired(false);
        }
    }
}
