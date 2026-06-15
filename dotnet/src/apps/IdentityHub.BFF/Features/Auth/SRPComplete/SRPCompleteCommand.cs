using MediatR;
using Shared.Kernel.Results;
using Shared.Contracts.Response.Auth;

namespace IdentityHub.BFF.Features.Auth.SRPComplete
{
    public sealed record SRPCompleteCommand(string TempToken) : IRequest<Result<CompleteSrpAuthResponse>>;
}