using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Domain.ValueObjects.User
{
    public readonly record struct Avatar
    {
        public string Value { get; }

        internal Avatar(string value) => Value = value;

        public static Result<Avatar> Create(string bucketName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(bucketName) && string.IsNullOrWhiteSpace(fileName))
                return Result<Avatar>.Failure(Error.Validation("Поля были пустые!"));
                
            var fullName = $"{bucketName}:{fileName}";

            return Result<Avatar>.Success(new Avatar(fullName));
        }

        public (string BucketName, string FileName) GetFullName()
        {
            var strings = Value.Split(':');

            var bucketName = strings[0];
            var fileName = strings[1];

            return (bucketName, fileName);
        }

        public string GetBucket()
        {
            var strings = Value.Split(':');

            var bucketName = strings[0];

            return bucketName;
        }

        public string GetFile()
        {
            var strings = Value.Split(':');

            var fileName = strings[1];

            return fileName;
        }
        
        public override string ToString() => Value;
        public static implicit operator string(Avatar value) => value.Value;
    }
}