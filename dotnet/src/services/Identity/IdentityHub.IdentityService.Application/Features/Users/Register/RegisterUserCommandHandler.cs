using IdentityHub.IdentityService.Domain.Enums;
using IdentityHub.IdentityService.Domain.Models;
using IdentityHub.IdentityService.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Application.Features.Users.Register
{
    public sealed class RegisterUserCommandHandler(IdentityContext identityContext) : IRequestHandler<RegisterUserCommand, Result>
    {
        private readonly IdentityContext _identityContext = identityContext;

        public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<Error>();

            try
            {
                var userResult = User.Create(request.Login, request.UserName, request.Email);
                User user = null!;

                userResult.Switch(onSuccess: createdUser => user = createdUser, onFailure: errors.AddRange);

                if (errors.Count > 0)
                    return Result.Failure(errors);

                user.AddSecureData(SecureDataType.MainDek, request.EncryptedDek, request.EncryptionAlgorithm, request.Iterations, request.KdfType).Switch(() => { }, errors.AddRange);

                user.AddAuthMethod(AuthType.SRP, request.Login, request.Verifier, request.ClientSalt).Switch(() => { }, errors.AddRange);
                user.AddAuthMethod(AuthType.Email, request.Email, authData: null, salt: null).Switch(() => { }, errors.AddRange);

                user.AddRole(SmartRole.User);

                if (!string.IsNullOrWhiteSpace(request.Phone))
                    user.AddAuthMethod(AuthType.NumberPhone, request.Phone, authData: null, salt: null).Switch(() => { }, errors.AddRange);

                if (errors.Count > 0)
                    return Result.Failure(errors);

                await _identityContext.Set<User>().AddAsync(user, cancellationToken);
                await _identityContext.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                return Result.Failure(Error.Conflict("Email уже занят."));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Result.Failure(Error.New(ErrorCode.Save, "Произошла критическая ошибка на стороне сервера при регистрации"));
            }
        }
    }
}
