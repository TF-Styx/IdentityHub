using IdentityHub.BFF.Clients.Auth;
using MediatR;
using Shared.Contracts.Request.SRP;
using Shared.Contracts.Response.SRP;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Features.Auth.SRPChallenge
{
    public sealed class SRPChallengeHandler(IAuthService authService) : IRequestHandler<SRPChallengeCommand, Result<SRPChallengeResponse>>
    {
        private readonly IAuthService _authService = authService;

        public async Task<Result<SRPChallengeResponse>> Handle(SRPChallengeCommand request, CancellationToken cancellationToken)
            => await _authService.SRPChallenge(new SRPChallengeRequest(request.Login));
    }
}