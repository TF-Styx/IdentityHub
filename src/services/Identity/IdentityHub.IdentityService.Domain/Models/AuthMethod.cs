using IdentityHub.IdentityService.Domain.Enums;
using IdentityHub.IdentityService.Domain.ValueObjects.AuthMethod;
using IdentityHub.IdentityService.Domain.ValueObjects.User;
using Shared.Kernel.Primitives;

namespace IdentityHub.IdentityService.Domain.Models
{
    public sealed class AuthMethod : Entity<AuthMethodId>
    {
        public UserId UserId { get; private set; }
        public AuthType AuthType { get; private set; }
        public AuthKey AuthKey { get; private set; }
        public AuthData? AuthData { get; private set; }
        public string? Salt { get; private set; }

        private AuthMethod() { }

        private AuthMethod
            (
                AuthMethodId id, 
                UserId userId, 
                AuthType authType, 
                AuthKey authKey, 
                AuthData? authData, 
                string? salt
            ) : base(id)
        {
            UserId = userId;
            AuthType = authType;
            AuthKey = authKey;
            AuthData = authData;
            Salt = salt;
        }

        internal static AuthMethod Create(UserId userId, AuthType authType, AuthKey authKey, AuthData? authData, string? salt)
            => new AuthMethod(AuthMethodId.New(), userId, authType, authKey, authData, salt);
    }
}
