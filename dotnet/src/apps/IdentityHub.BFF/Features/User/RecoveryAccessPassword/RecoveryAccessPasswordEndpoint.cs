using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.User;

namespace IdentityHub.BFF.Features.User.RecoveryAccessPassword
{
    public static class RecoveryAccessPasswordEndpoint
    {
        public static void MapRecoveryAccessPassword(this IEndpointRouteBuilder app)
        {
            app.MapPatch("recovery-access-password", async ([FromBody] RecoveryAccessPasswordRequest request, [FromServices] IMediator mediator, CancellationToken cancellationToken = default) =>
            {
                var command = new RecoveryAccessPasswordCommand
                    (
                        request.Login,
                        request.Verifier,
                        request.ClientSalt,
                        request.EncryptedDek,
                        request.EncryptionAlgorithm,
                        request.Iterations,
                        request.KdfType
                    );

                var result = await mediator.Send(command, cancellationToken);

                if (result.IsFailure)
                    return Results.BadRequest(result.Errors);

                return Results.Ok();
            });
        }
    }
}