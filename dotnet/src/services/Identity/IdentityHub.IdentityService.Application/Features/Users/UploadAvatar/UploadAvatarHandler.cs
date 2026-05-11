using MediatR;
using Shared.Kernel.Results;
using Microsoft.EntityFrameworkCore;
using IdentityHub.IdentityService.Domain.Models;
using IdentityHub.IdentityService.Application.Abstracts;
using IdentityHub.IdentityService.Domain.ValueObjects.User;

namespace IdentityHub.IdentityService.Application.Features.Users.UploadAvatar
{
    public sealed class UploadAvatarHandler(IApplicationDbContext context) : IRequestHandler<UploadAvatarCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Result> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Set<User>().FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

                if (user == null)
                    return Result.Failure(Error.NotFound("Пользователь"));

                var avatar = Avatar.Create(request.BucketName, request.FileName);

                if (avatar.IsFailure)
                    return Result.Failure(Error.New(ErrorCode.Upload, "Ошибка загрузки фотографии!"));

                user.AddAvatar(avatar.Value);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Result.Failure(Error.New(ErrorCode.Upload, "Произошла критическая ошибка на стороне сервера при сохранении аватарки!"));
            }
        }
    }
}