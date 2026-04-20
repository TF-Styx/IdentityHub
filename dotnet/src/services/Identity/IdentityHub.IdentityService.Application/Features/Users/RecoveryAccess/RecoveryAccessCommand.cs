using MediatR;
using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Application.Features.Users.RecoveryAccess
{
    public sealed record RecoveryAccessCommand
        (
            string Login,
            string Verifier,
            string ClientSalt,
            string EncryptedDek,
            string EncryptionAlgorithm,
            int Iterations,
            string KdfType
        ) : IRequest<Result>;
}