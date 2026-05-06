using MediatR;
using Shared.Kernel.Results;
using Shared.Contracts.Request.User;
using IdentityHub.BFF.Clients.Identity;

namespace IdentityHub.BFF.Features.Profile.Update.UpdateUserName
{
    public sealed class UpdateUserNameHandler(IIdentityService identityService) : IRequestHandler<UpdateUserNameCommand, Result>
    {
        private readonly IIdentityService _identityService = identityService;

        public async Task<Result> Handle(UpdateUserNameCommand request, CancellationToken cancellationToken)
            => await _identityService.UpdateUserNameAsync(new UpdateUserNameRequest(request.UserId, request.UserName));
    }
}