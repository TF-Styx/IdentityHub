using MediatR;
using Shared.Kernel.Results;
using Microsoft.EntityFrameworkCore;
using IdentityHub.IdentityService.Domain.Models;
using IdentityHub.IdentityService.Application.Abstracts;
using IdentityHub.IdentityService.Domain.ValueObjects.User;

namespace IdentityHub.IdentityService.Application.Features.Users.UpdateUserName
{
    public sealed class UpdateUserNameHandler(IApplicationDbContext context) : IRequestHandler<UpdateUserNameCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Result> Handle(UpdateUserNameCommand request, CancellationToken cancellationToken)
        {
            List<Error> errors = [];

            try
            {
                var user = await _context.Set<User>().FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

                if (user == null)
                    return Result.Failure(Error.NotFound("Пользователь"));

                var userNameResult = UserName.Create(request.UserName);

                if (userNameResult.IsFailure)
                    return userNameResult;

                user.ChangeUserName(userNameResult.Value);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Result.Failure(Error.New(ErrorCode.Update, "Произошла критическая ошибка на стороне сервера при изменении данных!"));
            }
        }
    }
}