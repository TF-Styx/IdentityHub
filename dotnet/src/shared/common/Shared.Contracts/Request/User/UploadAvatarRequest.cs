namespace Shared.Contracts.Request.User
{
    public sealed record UploadAvatarRequest(string UserId, string BucketName, string FileName);
}