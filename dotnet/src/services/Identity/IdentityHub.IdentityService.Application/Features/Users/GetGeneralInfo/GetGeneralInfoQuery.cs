using MediatR;
using Shared.Kernel.Results;
using Shared.Contracts.Response.User;

namespace IdentityHub.IdentityService.Application.Features.Users.GetGeneralInfo
{
    public sealed record GetGeneralInfoQuery(Guid UserId) : IRequest<Result<ProfileGeneralInfoResponse>>;
}