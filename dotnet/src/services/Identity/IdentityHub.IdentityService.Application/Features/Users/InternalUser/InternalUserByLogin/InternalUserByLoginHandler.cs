using IdentityHub.IdentityService.Domain.Enums;
using IdentityHub.IdentityService.Domain.Models;
using IdentityHub.IdentityService.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.User;
using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Application.Features.Users.InternalUser.InternalUserByLogin
{
    public sealed class InternalUserByLoginHandler(IdentityContext context) : IRequestHandler<InternalUserByLoginQuery, Result<UserResponse>>
    {
        private readonly IdentityContext _context = context;

        public async Task<Result<UserResponse>> Handle(InternalUserByLoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Set<User>()
                .Include(x => x.AuthMethods).Include(x => x.SecureDatas)
                    .FirstOrDefaultAsync(x => x.Login == request.Login, cancellationToken);

            if (user == null)
                return Result<UserResponse>.Failure(Error.NotFound("Пользователь не найден!"));

            var authData = user.AuthMethods.FirstOrDefault(x => x.AuthType == AuthType.SRP);

            if (authData == null)
                return Result<UserResponse>.Failure(Error.NotFound("Данные вашей учетной записи повреждены!"));

            var secureData = user.SecureDatas.FirstOrDefault(x => x.SecureDataType == SecureDataType.MainDek);

            if (secureData == null)
                return Result<UserResponse>.Failure(Error.NotFound("Данные вашей учетной записи повреждены!"));

            var response = new UserResponse(user.Id, user.Login, user.UserName, authData.AuthData, authData.Salt, secureData.SecureEncryptedValue, user.Email);

            return Result<UserResponse>.Success(response);
        }
    }
}