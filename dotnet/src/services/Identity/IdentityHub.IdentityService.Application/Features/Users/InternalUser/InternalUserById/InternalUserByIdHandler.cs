using MediatR;
using Shared.Kernel.Results;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.User;
using IdentityHub.IdentityService.Domain.Enums;
using IdentityHub.IdentityService.Domain.Models;
using IdentityHub.IdentityService.Application.Abstracts;

namespace IdentityHub.IdentityService.Application.Features.Users.InternalUser.InternalUserById
{
    public sealed class InternalUserByIdHandler(IApplicationDbContext context) : IRequestHandler<InternalUserByIdQuery, Result<UserResponse>>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Result<UserResponse>> Handle(InternalUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Set<User>()
                .Include(x => x.AuthMethods).Include(x => x.SecureDatas)
                    .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

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