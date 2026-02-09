using IdentityHub.IdentityService.Domain.Models;
using IdentityHub.IdentityService.Domain.ValueObjects.Status;
using IdentityHub.IdentityService.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityHub.IdentityService.Infrastructure.Persistence.Configurations
{
    internal sealed class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.ToTable("Statuses");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever()
                .HasConversion(statusId => statusId.Value, dbValue => new StatusId(dbValue));

            builder.Property(s => s.Name)
                .HasColumnName("Name")
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .HasConversion(name => name.Value, dbValue => new StatusName(dbValue));
        }
    }
}
