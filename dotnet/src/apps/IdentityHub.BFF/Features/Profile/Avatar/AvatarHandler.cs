using MediatR;
using Shared.Kernel.Results;
using IdentityHub.BFF.Clients.File;
using IdentityHub.BFF.Clients.Identity;
using IdentityHub.BFF.Constants;
using Shared.Contracts.Request.User;

namespace IdentityHub.BFF.Features.Profile.Avatar
{
    public sealed class AvatarHandler(IFileService fileService, IIdentityService identityService) : IRequestHandler<AvatarCommand, Result>
    {
        private readonly IFileService _fileService = fileService;
        private readonly IIdentityService _identityService = identityService;

        public async Task<Result> Handle(AvatarCommand request, CancellationToken cancellationToken)
        {
            var avatarResult = await _fileService.UploadAvatarAsync(Buckets.IDENTITY_HUB, request.File, request.ContentType, request.FileName, request.UserId);

            if (avatarResult.IsFailure)
                return Result.Failure(Error.New(ErrorCode.Upload, "Не удалось загрузить изображение!"));

            var dbResult = await _identityService.UploadAvatar(new UploadAvatarRequest(request.UserId, Buckets.IDENTITY_HUB, avatarResult.Value.FileName));

            if (dbResult.IsFailure)
                return Result.Failure(Error.New(ErrorCode.Upload, "Не удалось загрузить изображение!"));

            return Result.Success();
        }
    }
}