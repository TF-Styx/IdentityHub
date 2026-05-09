using MediatR;
using Shared.Kernel.Results;
using IdentityHub.BFF.Clients.File;
using IdentityHub.BFF.Clients.Identity;

namespace IdentityHub.BFF.Features.Profile.Avatar
{
    public sealed class AvatarHandler(IFileService fileService, IIdentityService identityService) : IRequestHandler<AvatarCommand, Result>
    {
        private readonly IFileService _fileService = fileService;
        private readonly IIdentityService _identityService = identityService;

        public async Task<Result> Handle(AvatarCommand request, CancellationToken cancellationToken)
        {
            return await _fileService.UploadAvatarAsync(request.File, request.ContentType, request.FileName, request.UserId);
        }
    }
}