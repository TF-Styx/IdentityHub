using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityHub.BFF.Features.User.GenerateCode
{
    public static class GenerateCodeEndpoint
    {
        public static void MapGenerateCode(this IEndpointRouteBuilder app)
        {
            app.MapPost("generate-code/{login}", async ([FromRoute] string login, [FromServices] IMediator mediator, CancellationToken cancellationToken = default) =>
            {
                var command = new GenerateCodeCommand(login);

                var result = await mediator.Send(command, cancellationToken);

                if (result.IsFailure)
                    return Results.BadRequest(result.Errors);

                return Results.Ok();
            });
        }
    }
}