using IdentityHub.IdentityService.Domain.Models;
using IdentityHub.IdentityService.Domain.ValueObjects.Role;
using IdentityHub.IdentityService.Infrastructure.Persistence.Contexts;
using MediatR;
using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Application.Features.Roles.Create
{
    public sealed class CreateRoleHandler(IdentityContext context) : IRequestHandler<CreateRoleCommand, Result>
    {
        private readonly IdentityContext _context = context;

        public async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<Error>();

            try
            {
                var roleName = RoleName.Create(request.Name)
                    .Match
                    (
                        rm => rm, 
                        error => 
                        { 
                            errors.AddRange(error); 
                            return RoleName.Empty().Value; 
                        }
                    );

                var role = Role.Create(roleName)
                    .Match
                    (
                        r => r,
                        error =>
                        {
                            errors.AddRange(error);
                            return Role.Empty().Value;
                        }
                    );

                if (errors.Count() > 0)
                    return Result.Failure(errors);

                await _context.Set<Role>().AddAsync(role, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
