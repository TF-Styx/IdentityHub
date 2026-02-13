using MediatR;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Features.Registration
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