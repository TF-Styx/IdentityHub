using IdentityHub.IdentityService.Domain.Enums;
using IdentityHub.IdentityService.Domain.ValueObjects.SecureData;
using IdentityHub.IdentityService.Domain.ValueObjects.User;
using Shared.Kernel.Primitives;

namespace IdentityHub.IdentityService.Domain.Models
{
    public sealed class SecureData : Entity<SecureDataId>
    {
        public UserId UserId { get; private set; }
        public SecureDataType SecureDataType { get; private set; }
        public EncryptedValue SecureEncryptedValue { get; private set; }
        public EncryptedMetadata SecureEncryptedMetadata { get; private set; }

        private SecureData() { }
        private SecureData
            (
                SecureDataId id, 
                UserId userId, 
                SecureDataType secureDataType, 
                EncryptedValue secureEncryptedValue, 
                EncryptedMetadata secureEncryptedMetadata
            ) : base(id)
        {
            UserId = userId;
            SecureDataType = secureDataType;
            SecureEncryptedValue = secureEncryptedValue;
            SecureEncryptedMetadata = secureEncryptedMetadata;
        }

        internal static SecureData Create
            (
                UserId userId, 
                SecureDataType secureDataType, 
                EncryptedValue secureEncryptedValue, 
                EncryptedMetadata secureEncryptedMetadata
            )
            => new SecureData
                (
                    SecureDataId.New(), 
                    userId, 
                    secureDataType, 
                    secureEncryptedValue, 
                    secureEncryptedMetadata
                );
    }
}
