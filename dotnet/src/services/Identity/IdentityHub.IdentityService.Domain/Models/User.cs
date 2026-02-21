using IdentityHub.IdentityService.Domain.Enums;
using IdentityHub.IdentityService.Domain.ValueObjects.AuthMethod;
using IdentityHub.IdentityService.Domain.ValueObjects.Role;
using IdentityHub.IdentityService.Domain.ValueObjects.SecureData;
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
        public Email Email { get; private set; }
        public NumberPhone? NumberPhone { get; private set; }
        public DateTime DateRegistration { get; private set; }
        public DateTime? DateEntry { get; private set; }
        public DateTime DateUpdate { get; private set; }

        public StatusId StatusId { get; private set; }

        private readonly List<UserRoles> _userRoles = [];
        public IReadOnlyCollection<UserRoles> UserRoles => _userRoles.AsReadOnly();

        private readonly List<AuthMethod> _authMethods = [];
        public IReadOnlyCollection<AuthMethod> AuthMethods => _authMethods.AsReadOnly();

        private readonly List<SecureData> _secureDatas = [];
        public IReadOnlyCollection<SecureData> SecureDatas => _secureDatas.AsReadOnly();

        private User() { }
        private User(UserId id, Login login, UserName userName, Email mail) : base(id)
        {
            Login = login;
            UserName = userName;
            Email = mail;
            DateRegistration = DateTime.UtcNow;
            StatusId = StatusId.Create(Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479")).Value;
        }

        public static Result<User> Create(string login, string userName, string mail)
        {
            var loginResult = Login.Create(login);
            var userNameResult = UserName.Create(userName);
            var mailResult = Email.Create(mail);

            if (loginResult.IsFailure || userNameResult.IsFailure || mailResult.IsFailure)
            {
                var errors = new List<Error>();

                errors.AddRange(loginResult.Errors);
                errors.AddRange(userNameResult.Errors);
                errors.AddRange(mailResult.Errors);

                return Result<User>.Failure(errors);
            }

            var user = new User
                (
                    UserId.New(),
                    loginResult.Value,
                    userNameResult.Value,
                    mailResult.Value
                );

            return Result<User>.Success(user);
        }

        #region User

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

        public void ChangeMail(Email mail)
        {
            Email = mail;
            UpdateDate();
        }

        public void UpdateDateEntry()
            => DateEntry = DateTime.UtcNow;

        private void UpdateDate()
            => DateUpdate = DateTime.UtcNow;

        #endregion

        #region Role

        public IEnumerable<Error> AddRole(params SmartRole[] roles)
        {
            var errors = new List<Error>();

            foreach (var item in roles)
            {
                if (_userRoles.Where(x => x.RoleId == item.Id).Count() > 0)
                {
                    errors.Add(Error.New(ErrorCode.Conflict, $"Роль '{item.Name}' уже есть!"));
                    continue;
                }
                
                _userRoles.Add(Models.UserRoles.Create(this.Id, item.Id));
            }

            UpdateDate();

            return errors;
        }

        public void RemoveRole(RoleId roleId)
        {
            var userRole = _userRoles.FirstOrDefault(x => x.RoleId == roleId);

            if (userRole == null)
                return;

            _userRoles.Remove(userRole);
            UpdateDate();
        }

        #endregion

        #region AuthMethod

        public Result AddAuthMethod(AuthType authType, string authKey, string? authData, string? salt)
        {
            if(_authMethods.Any(x => x.AuthType == authType))
                return Result.Failure(Error.Conflict($"Данный метод аутентификации '{authType}' ужа зарегестирован, для вашего аккаунта!"));

            var errors = new List<Error>();

            var authKeyResult = AuthKey.Create(authKey);
            var authDataResult = authData != null ? AuthData.Create(authData) : null;

            authKeyResult.Switch(onSuccess: _ => { }, onFailure: errors.AddRange);
            authDataResult?.Switch(onSuccess: _ => { }, onFailure: err => errors.AddRange(err));

            if (errors.Count > 0)
                return Result.Failure(errors);

            var authMethod = AuthMethod.Create(this.Id, authType, authKeyResult.Value, authDataResult?.Value, salt);

            _authMethods.Add(authMethod);
            UpdateDate();

            return Result.Success();
        }

        #endregion

        #region SecureData

        public Result AddSecureData(SecureDataType secureDataType, string secureEncryptedValue, string algoritm, int interation, string kdfType)
        {
            if (_secureDatas.Any(x => x.SecureDataType == secureDataType))
                return Result.Failure(Error.Conflict($"Данный тип секретных данных '{secureDataType}' ужа зарегестирован, для вашего аккаунта!"));

            var errors = new List<Error>();

            var secureEncryptedValueResult = EncryptedValue.Create(secureEncryptedValue);
            var secureEncryptedMetadataResult = EncryptedMetadata.Create(algoritm, interation, kdfType);

            secureEncryptedValueResult.Switch(onSuccess: _ => { }, onFailure: errors.AddRange);
            secureEncryptedMetadataResult.Switch(onSuccess: _ => { }, onFailure: errors.AddRange);

            if (errors.Count > 0)
                return Result.Failure(errors);

            var secureData = SecureData.Create(this.Id, secureDataType, secureEncryptedValueResult.Value, secureEncryptedMetadataResult.Value);

            _secureDatas.Add(secureData);
            UpdateDate();

            return Result.Success();
        }

        #endregion
    }
}
