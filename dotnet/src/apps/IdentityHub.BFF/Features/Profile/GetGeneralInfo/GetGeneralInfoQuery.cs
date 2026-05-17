using MediatR;
using Shared.Contracts.Response.BFF;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Features.Profile.GetGeneralInfo
{
    public sealed record GetGeneralInfoQuery(string UserId) : IRequest<Result<ProfileGeneralInfoBFFResponse?>>;
}