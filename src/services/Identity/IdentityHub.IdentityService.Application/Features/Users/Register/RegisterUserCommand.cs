using IdentityHub.IdentityService.Domain.Models;
using MediatR;
using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Application.Features.Users.Register
{
    public sealed record RegisterUserCommand(string Login, string UserName, string Password, string ClientSalt, string EncryptedDek, string Mail, List<Guid> RoleIds) : IRequest<Result>;
}
