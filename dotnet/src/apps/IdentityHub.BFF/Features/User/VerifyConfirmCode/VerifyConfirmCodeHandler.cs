using IdentityHub.BFF.Clients.Identity;
using MediatR;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Features.User.VerifyConfirmCode
{
    public sealed class VerifyConfirmCodeHandler(IIdentityService identityService) : IRequestHandler<VerifyConfirmCodeCommand, Result>
    {
        private readonly IIdentityService _identityService = identityService;

        public async Task<Result> Handle(VerifyConfirmCodeCommand request, CancellationToken cancellationToken)
            => await _identityService.VerifyConfirmCodeAsync(request.Login, request.Code);
    }
}