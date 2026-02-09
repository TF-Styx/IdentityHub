using IdentityHub.IdentityService.Domain.Models;
using IdentityHub.IdentityService.Domain.ValueObjects.SecureData;
using IdentityHub.IdentityService.Domain.ValueObjects.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace IdentityHub.IdentityService.Infrastructure.Persistence.Configurations
{
    internal sealed class SecureDataConfiguration : IEntityTypeConfiguration<SecureData>
    {
        public void Configure(EntityTypeBuilder<SecureData> builder)
        {
            builder.ToTable("SecureData");
            builder.HasKey(sd => sd.Id);

            builder.Property(sd => sd.Id)
                   .HasColumnName("Id")
                   .ValueGeneratedNever()
                   .HasConversion(outBD => outBD.Value, inDB => new SecureDataId(inDB));

            builder.Property(sd => sd.UserId)
                   .HasColumnName("UserId")
                   .HasConversion(outDB => outDB.Value, inDB => new UserId(inDB));

            builder.Property(sd => sd.SecureDataType)
                   .HasColumnName("SecureDataType")
                   .HasConversion<int>();

            builder.Property(sd => sd.SecureEncryptedValue)
                   .HasColumnName("SecureEncryptedValue")
                   .HasConversion(outDB => outDB.Value, inDB => new EncryptedValue(inDB));

            builder.Property(sd => sd.SecureEncryptedMetadata)
                   .HasColumnName("SecureEncryptedMetadata")
                   .HasColumnType("jsonb")
                   .HasConversion
                        (
                            outDB => JsonSerializer.Serialize(outDB, (JsonSerializerOptions?)null), 
                            inDB => JsonSerializer.Deserialize<EncryptedMetadata>(inDB, (JsonSerializerOptions?)null)
                        );
        }
    }
}
