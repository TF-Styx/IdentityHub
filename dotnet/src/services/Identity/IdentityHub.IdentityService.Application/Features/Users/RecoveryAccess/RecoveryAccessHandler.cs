using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityHub.IdentityService.Application.Abstracts;
using IdentityHub.IdentityService.Domain.Enums;
using IdentityHub.IdentityService.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Application.Features.Users.RecoveryAccess
{
    public sealed class RecoveryAccessHandler(IApplicationDbContext identityContext) : IRequestHandler<RecoveryAccessCommand, Result>
    {
        private readonly IApplicationDbContext _identityContext = identityContext;

        public async Task<Result> Handle(RecoveryAccessCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<Error>();

            try
            {
                var user = await _identityContext.Set<User>()
                    .Include(x => x.AuthMethods)
                    .Include(x => x.SecureDatas)
                        .FirstOrDefaultAsync(x => x.Login == request.Login, cancellationToken);

                user!.UpdateMainDek(request.EncryptedDek, request.EncryptionAlgorithm, request.Iterations, request.KdfType).Switch(() => { }, errors.AddRange);

                user!.UpdateSRP(request.Verifier, request.ClientSalt).Switch(() => { }, errors.AddRange);

                if (errors.Count > 0)
                    return Result.Failure(errors);

                await _identityContext.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Result.Failure(Error.New(ErrorCode.Save, "Произошла критическая ошибка на стороне сервера при регистрации"));
            }
        }
    }
}