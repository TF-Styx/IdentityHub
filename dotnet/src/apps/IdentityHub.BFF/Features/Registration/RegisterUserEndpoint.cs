using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.User;

namespace IdentityHub.BFF.Features.Registration
{
    public static class RegisterUserEndpoint
    {
        public static void MapRegistration(this IEndpointRouteBuilder app)
        {
            app.MapPost("registration", async ([FromBody] RegisterUserRequest request, [FromServices] IMediator mediator, CancellationToken cancellationToken = default) =>
            {
                var command = new RegisterUserCommand
                    (
                        request.Login,
                        request.UserName,
                        request.Verifier,
                        request.ClientSalt,
                        request.EncryptedDek,
                        request.EncryptionAlgorithm,
                        request.Iterations,
                        request.KdfType,
                        request.Email,
                        request.Phone
                    );

                var result = await mediator.Send(command, cancellationToken);

                if (result.IsFailure)
                    return Results.BadRequest(result.Errors);

                return Results.Ok();
            });
        }
    }
}