using Shared.Kernel.Results;
using Shared.Contracts.Response.Avatar;

namespace IdentityHub.BFF.Clients.File
{
    public interface IFileService
    {
        Task<Result<AvatarResponse>> UploadAvatarAsync(string bucketName, Stream fileStream, string contentType, string fileName, string userId);
    }
}