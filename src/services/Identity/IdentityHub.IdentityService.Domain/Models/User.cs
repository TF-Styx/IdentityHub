using IdentityHub.IdentityService.Domain.ValueObjects.Role;
using IdentityHub.IdentityService.Domain.ValueObjects.Status;
using IdentityHub.IdentityService.Domain.ValueObjects.User;
using Shared.Kernel.Primitives;
using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Domain.Models
{
    public sealed class User : AggregateRoot<UserId>
    {
        public Login Login { get; private set; }
        public UserName UserName { get; private set; }
        public HashPassword HashPassword { get; private set; }
        public string ClientSalt { get; private set; } = null!;
        public string EncryptedDek { get; private set; } = null!;
        public Mail Mail { get; private set; }
        public DateTime DateRegistration { get; private set; }
        public DateTime? DateEntry { get; private set; }
        public DateTime DateUpdate { get; private set; }

        public StatusId StatusId { get; private set; }

        private readonly List<RoleId> _roleIds = [];
        public IReadOnlyCollection<RoleId> RoleIds => _roleIds.AsReadOnly();

        private User() { }
        private User(UserId id, UserName userName, HashPassword hashPassword, string clientSalt, string encryptedDek, Mail mail) : base(id)
        {
            UserName = userName;
            HashPassword = hashPassword;
            ClientSalt = clientSalt;
            EncryptedDek = encryptedDek;
            Mail = mail;
            DateRegistration = DateTime.UtcNow;
        }

        public static Result<User> Create(string userName, string hashPassword, string clientSalt, string encryptedDek, string mail)
        {
            var userNameResult = UserName.Create(userName);
            var hashResult = HashPassword.Create(hashPassword);
            var mailResult = Mail.Create(mail);

            if (userNameResult.IsFailure || hashResult.IsFailure || mailResult.IsFailure)
            {
                var errors = new List<Error>();

                errors.AddRange(userNameResult.Errors);
                errors.AddRange(hashResult.Errors);
                errors.AddRange(mailResult.Errors);

                return Result<User>.Failure(errors);
            }

            var user = new User
                (
                    UserId.New(),
                    userNameResult.Value,
                    hashResult.Value,
                    clientSalt,
                    encryptedDek,
                    mailResult.Value
                );

            return Result<User>.Success(user);
        }

        public void AddRole(RoleId roleId)
        {
            if (!_roleIds.Contains(roleId))
                _roleIds.Add(roleId);
        }

        public void RemoveRole(RoleId roleId)
        {
            _roleIds.Remove(roleId);
            UpdateDate();
        }

        public void ChangeStatus(StatusId statusId)
        {
            StatusId = statusId;
            UpdateDate();
        }

        public void ChangeUserName(UserName userName)
        {
            UserName = userName;
            UpdateDate();
        }

        public void ChangeHashPassword(HashPassword hashPassword)
        {
            HashPassword = hashPassword;
            UpdateDate();
        }

        public void ChangeMail(Mail mail)
        {
            Mail = mail;
            UpdateDate();
        }

        public void UpdateDateEntry()
            => DateEntry = DateTime.UtcNow;

        private void UpdateDate()
            => DateUpdate = DateTime.UtcNow;
    }
}
