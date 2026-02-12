using Shared.Kernel.Primitives;

namespace IdentityHub.AuthService.Domain.Models
{
    public sealed class Token : AggregateRoot<Guid>
    {
        public Guid UserId { get; private set; }
        public string RefrashToken { get; private set; } = null!;
        public string AccessToken { get; private set; } = null!;
        public DateTime IssueDate { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public bool IsUsed { get; private set; }

        private Token() { }
        private Token
            (
                Guid id, 
                Guid userId, 
                string refrashToken, 
                string accessToken, 
                DateTime issueDate, 
                DateTime expiryDate, 
                bool idUsed
            ) : base(id)
        {
            UserId = userId;
            RefrashToken = refrashToken;
            AccessToken = accessToken;
            IssueDate = issueDate;
            ExpiryDate = expiryDate;
            IsUsed = idUsed;
        }

        public static Token Create
            (
                Guid userId, 
                string refrashToken, 
                string accessToken, 
                DateTime issueDate, 
                DateTime expiryDate, 
                bool idUsed
            ) => new Token
                (
                    Guid.NewGuid(),
                    userId,
                    refrashToken,
                    accessToken,
                    issueDate,
                    expiryDate,
                    idUsed
                );

        public void MarkAsUsed() => IsUsed = true;
    }
}
