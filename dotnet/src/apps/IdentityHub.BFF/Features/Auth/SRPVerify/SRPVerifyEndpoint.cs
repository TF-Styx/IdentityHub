using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.SRP;

namespace IdentityHub.BFF.Features.Auth.SRPVerify
{
    public static class SRPVerifyEndpoint
    {
        public static void MapSRPVerify(this IEndpointRouteBuilder app)
        {
            app.MapPost("auth/srp/verify", async (HttpContext httpContext, [FromBody] SRPVerifyRequest request, [FromServices] IMediator mediator, CancellationToken cancellationToken = default) =>
            {
                var command = new SRPVerifyCommand(request.Login, request.A, request.M1);

                var result = await mediator.Send(command, cancellationToken);

                if (result.IsFailure)
                    return Results.BadRequest(result.Errors);

                return Results.Ok(result.Value);
            });
        }
    }
}