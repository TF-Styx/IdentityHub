using IdentityHub.IdentityService.Domain.Models;
using IdentityHub.IdentityService.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Results;
using Shared.Security.Hashers;

namespace IdentityHub.IdentityService.Application.Features.Users.Register
{
    public sealed class RegisterUserCommandHandler(IdentityContext identityContext, IPasswordHasher passwordHasher) : IRequestHandler<RegisterUserCommand, Result>
    {
        private readonly IdentityContext _identityContext = identityContext;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var hashPassword = _passwordHasher.Hash(request.Password);

                var user = User.Create(request.UserName, hashPassword, request.ClientSalt, request.EncryptedDek, request.Mail);

                if (user.IsFailure)
                    return Result.Failure(user.Errors);

                await _identityContext.Set<User>().AddAsync(user.Value, cancellationToken);
                await _identityContext.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                return Result.Failure(Error.Conflict("Email уже занят."));
            }
            catch (Exception)
            {
                return Result.Failure(Error.New(ErrorCode.Save, "Произошла критическая ошибка на стороне сервера при регистрации"));
            }
        }
    }
}
