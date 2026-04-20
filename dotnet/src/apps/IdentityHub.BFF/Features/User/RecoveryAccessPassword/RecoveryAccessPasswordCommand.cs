using MediatR;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Features.User.RecoveryAccessPassword
{
    public sealed record RecoveryAccessPasswordCommand
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