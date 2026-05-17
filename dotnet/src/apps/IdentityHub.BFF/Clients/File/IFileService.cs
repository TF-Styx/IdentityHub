using Shared.Kernel.Results;
using Shared.Contracts.Response.Avatar;
using Shared.Contracts.Request.Avatar;

namespace IdentityHub.BFF.Clients.File
{
    public interface IFileService
    {
        Task<Result<PresignedUrlResponse?>> GetPresignedUrlAsync(PresignedUrlRequest request);
        Task<Result<AvatarResponse>> UploadAvatarAsync(string bucketName, Stream fileStream, string contentType, string fileName, string userId);
    }
}