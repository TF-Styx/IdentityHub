using MediatR;
using Shared.Contracts.Response.SRP;
using Shared.Kernel.Results;

namespace IdentityHub.AuthService.Application.Features.SRPChallenge
{
    public sealed record SRPChallengeCommand(string Login) : IRequest<Result<SRPChallengeResponse>>;
}
