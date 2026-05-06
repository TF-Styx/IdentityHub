using MediatR;
using Shared.Kernel.Results;
using Shared.Contracts.Request.User;
using IdentityHub.BFF.Clients.Identity;

namespace IdentityHub.BFF.Features.User.RecoveryAccessPassword
{
    public sealed class RecoveryAccessPasswordHandler(IIdentityService identityService) : IRequestHandler<RecoveryAccessPasswordCommand, Result>
    {
        private readonly IIdentityService _identityService = identityService;

        public async Task<Result> Handle(RecoveryAccessPasswordCommand request, CancellationToken cancellationToken)
            => await _identityService.RecoveryAccessPasswordAsync(new RecoveryAccessPasswordRequest
                (
                    request.Login,
                    request.Verifier,
                    request.ClientSalt,
                    request.EncryptedDek,
                    request.EncryptionAlgorithm,
                    request.Iterations,
                    request.KdfType
                ));
    }
}