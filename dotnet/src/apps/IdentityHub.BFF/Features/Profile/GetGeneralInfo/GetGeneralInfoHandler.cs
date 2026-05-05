using IdentityHub.BFF.Clients.Identity;
using MediatR;
using Shared.Contracts.Response.User;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Features.Profile.GetGeneralInfo
{
    public sealed class GetGeneralInfoHandler(IIdentityService identityService) : IRequestHandler<GetGeneralInfoQuery, Result<ProfileGeneralInfoResponse?>>
    {
        private readonly IIdentityService _identityService = identityService;

        public async Task<Result<ProfileGeneralInfoResponse?>> Handle(GetGeneralInfoQuery request, CancellationToken cancellationToken)
            => await _identityService.GetGeneralInfoAsync(request.UserId);
    }
}