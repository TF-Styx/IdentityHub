using IdentityHub.BFF.Clients.Identity;
using MediatR;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Features.User.GenerateCode
{
    public sealed class GenerateCodeHandler(IIdentityService identityService) : IRequestHandler<GenerateCodeCommand, Result>
    {
        private readonly IIdentityService _identityService = identityService;

        public async Task<Result> Handle(GenerateCodeCommand request, CancellationToken cancellationToken)
            => await _identityService.GenerateCodeAsync(request.Login);
    }
}