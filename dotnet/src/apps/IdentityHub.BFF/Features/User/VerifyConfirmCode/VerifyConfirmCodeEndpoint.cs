using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityHub.BFF.Features.User.VerifyConfirmCode
{
    public static class VerifyConfirmCodeEndpoint
    {
        public static void MapVerifyConfirmCode(this IEndpointRouteBuilder app)
        {
            app.MapPost("verify-confirm-code/{login}/{code}", async ([FromRoute] string login, [FromRoute] int code, [FromServices] IMediator mediator, CancellationToken cancellationToken = default) =>
            {
                var command = new VerifyConfirmCodeCommand(login, code);

                var result = await mediator.Send(command, cancellationToken);

                if (result.IsFailure)
                    return Results.BadRequest(result.Errors);

                return Results.Ok();
            });
        }
    }
}