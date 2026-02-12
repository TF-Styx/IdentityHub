using IdentityHub.AuthService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityHub.AuthService.Infrastructure.Persistence.Configurations
{
    public class TokenConfiguration : IEntityTypeConfiguration<Token>
    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            builder.ToTable("Tokens");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.UserId);

            builder.Property(t => t.RefrashToken);

            builder.Property(t => t.AccessToken);

            builder.Property(t => t.IssueDate);

            builder.Property(t => t.ExpiryDate);

            builder.Property(t => t.IsUsed);
        }
    }
}
