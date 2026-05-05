using MediatR;
using Shared.Contracts.Response.User;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Features.Profile.GetGeneralInfo
{
    public sealed record GetGeneralInfoQuery(string UserId) : IRequest<Result<ProfileGeneralInfoResponse?>>;
}