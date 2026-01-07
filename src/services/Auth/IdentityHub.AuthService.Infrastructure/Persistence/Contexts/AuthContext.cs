using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IdentityHub.AuthService.Infrastructure.Persistence.Contexts
{
    public sealed class AuthContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
