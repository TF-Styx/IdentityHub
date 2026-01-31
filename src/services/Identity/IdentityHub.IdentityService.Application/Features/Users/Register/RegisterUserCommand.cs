using MediatR;
using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Application.Features.Users.Register
{
    public sealed record RegisterUserCommand
        (
            string Login,
            string UserName,
            string Verifier,
            string ClientSalt,
            string EncryptedDek,
            string EncryptionAlgorithm,
            int Iterations,
            string KdfType,
            string Email, string? Phone
        ) : IRequest<Result>;
}
