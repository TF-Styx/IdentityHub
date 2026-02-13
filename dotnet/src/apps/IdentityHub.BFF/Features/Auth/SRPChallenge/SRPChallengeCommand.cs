using MediatR;
using Shared.Contracts.Response.SRP;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Features.Auth.SRPChallenge
{
    public sealed record SRPChallengeCommand(string Login) : IRequest<Result<SRPChallengeResponse>>;
}