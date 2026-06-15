using MediatR;
using Shared.Kernel.Results;
using Shared.Contracts.Response.SRP;

namespace IdentityHub.BFF.Features.Auth.SRPVerify
{
    public sealed record SRPVerifyCommand(string Login, string A, string M1) : IRequest<Result<SRPVerifyProofResponse>>;
}