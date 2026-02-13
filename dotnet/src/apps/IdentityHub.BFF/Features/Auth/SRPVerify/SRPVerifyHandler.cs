using IdentityHub.BFF.Clients.Auth;
using MediatR;
using Shared.Contracts.Request.SRP;
using Shared.Contracts.Response.Auth;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Features.Auth.SRPVerify
{
    public class SRPVerifyHandler(IAuthService authService) : IRequestHandler<SRPVerifyCommand, Result<AuthResponse>>
    {
        private readonly IAuthService _authService = authService;

        public async Task<Result<AuthResponse>> Handle(SRPVerifyCommand request, CancellationToken cancellationToken)
            => await _authService.SRPVerify(new SRPVerifyRequest(request.Login, request.A, request.M1));
    }
}