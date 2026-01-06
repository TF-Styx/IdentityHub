using IdentityHub.IdentityService.Domain.Models;
using IdentityHub.IdentityService.Domain.ValueObjects.Role;
using IdentityHub.IdentityService.Domain.ValueObjects.User;
using IdentityHub.IdentityService.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityHub.IdentityService.Infrastructure.Persistence.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever()
                .HasConversion(userId => userId.Value, dbValue => UserId.Create(dbValue).Value);

            builder.Property(s => s.Login)
                .HasColumnName("Login")
                .HasMaxLength(Login.MAX_LENGTH)
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .HasConversion(login => login.Value, dbValue => Login.Create(dbValue).Value);

            builder.Property(s => s.UserName)
                .HasColumnName("Name")
                .HasMaxLength(UserName.MAX_LENGTH)
                .HasConversion(name => name.Value, dbValue => UserName.Create(dbValue).Value);

            builder.Property(s => s.HashPassword)
                .HasColumnName("HashPassword")
                .HasConversion(hashPassword => hashPassword.Value, dbValue => HashPassword.Create(dbValue).Value);

            builder.Property(s => s.Mail)
                .HasColumnName("Mail")
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .HasConversion(mail => mail.Value, dbValue => Mail.Create(dbValue).Value);

            builder.Property(s => s.DateRegistration)
                .HasColumnName("DateRegistration");

            builder.Property(s => s.DateEntry)
                .HasColumnName("DateEntry");

            builder.Property(s => s.DateUpdate)
                .HasColumnName("DateUpdate");

            builder.HasOne<Status>().WithMany().HasForeignKey(user => user.StatusId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.RoleIds)
                   .HasColumnName("RoleIds")
                   .HasColumnType("uuid[]")
                   .HasField("_roleIds")
                   .UsePropertyAccessMode(PropertyAccessMode.Field)
                   .HasConversion
                        (
                            roleIds => roleIds.Select(roleId => roleId.Value).ToArray(),
                            dbValue => (dbValue ?? Array.Empty<Guid>()).Select(roleId => RoleId.Create(roleId).Value).ToList()
                        )
                   .Metadata.SetValueComparer(new ValueComparer<IReadOnlyCollection<RoleId>>
                        (
                            (c1, c2) => c1!.SequenceEqual(c2!),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToList()
                        ));
        }
    }
}
