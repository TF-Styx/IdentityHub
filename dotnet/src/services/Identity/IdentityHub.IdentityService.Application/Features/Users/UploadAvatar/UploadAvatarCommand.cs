using MediatR;
using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Application.Features.Users.UploadAvatar
{
    public sealed record UploadAvatarCommand(Guid UserId, string BucketName, string FileName) : IRequest<Result>;
}