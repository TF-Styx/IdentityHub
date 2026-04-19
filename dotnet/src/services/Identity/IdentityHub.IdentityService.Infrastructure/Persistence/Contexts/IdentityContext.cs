using IdentityHub.IdentityService.Application.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IdentityHub.IdentityService.Infrastructure.Persistence.Contexts
{
    public sealed class IdentityContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions), IApplicationDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
