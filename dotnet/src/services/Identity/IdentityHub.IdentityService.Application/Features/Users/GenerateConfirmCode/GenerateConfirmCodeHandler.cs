// using MediatR;
// using Shared.Kernel.Results;
// using IdentityHub.IdentityService.Infrastructure.Persistence.Contexts;
// using IdentityHub.IdentityService.Domain.Models;
// using Microsoft.EntityFrameworkCore;

// namespace IdentityHub.IdentityService.Application.Features.Users.GenerateConfirmCode
// {
//     public sealed class GenerateConfirmCodeHandler(IApplicationDbContext context) : IRequestHandler<GenerateConfirmCodeCommand, Result>
//     {
//         private readonly IApplicationDbContext _context = context;

//         public async Task<Result> Handle(GenerateConfirmCodeCommand request, CancellationToken cancellationToken)
//         {
//             var user = await _context.Set<User>().FirstOrDefaultAsync(x => x.Login == request.Login, cancellationToken);

//             if (user == null)
//                 return Result.Failure(Error.NotFound("Пользователь не найден!"));

            
//         }
//     }
// }