namespace Shared.Contracts.Request.Avatar
{
    public sealed record PresignedUrlRequest(string BucketName, string FileName);
}