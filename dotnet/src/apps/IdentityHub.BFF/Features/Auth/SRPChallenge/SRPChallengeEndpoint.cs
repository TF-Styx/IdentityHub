using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.SRP;

namespace IdentityHub.BFF.Features.Auth.SRPChallenge
{
    public static class SRPChallengeEndpoint
    {
        public static void MapSRPChallenge(this IEndpointRouteBuilder app)
        {
            app.MapPost("auth/srp/challenge", async ([FromBody] SRPChallengeRequest request, [FromServices] IMediator mediator, CancellationToken cancellationToken = default) =>
            {
                var command = new SRPChallengeCommand(request.Login);

                var result = await mediator.Send(command, cancellationToken);

                if (result.IsFailure)
                    return Results.BadRequest(result.Errors);

                return Results.Ok(result.Value);
            });
        }
    }
}