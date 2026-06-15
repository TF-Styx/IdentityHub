using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.User;

namespace IdentityHub.BFF.Features.Profile.Update.UpdateUserName
{
    public static class UpdateUserNameEndpoint
    {
        public static void MapUpdateUserName(this IEndpointRouteBuilder app)
        {
            app.MapPatch("update-name", async (HttpContext httpContext, [FromBody] UpdateUserNameRequest request, [FromServices] IMediator mediator) =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                    return Results.Unauthorized();

                var command = new UpdateUserNameCommand(userId, request.UserName);

                var result = await mediator.Send(command);

                return Results.Ok();
            }).RequireAuthorization();
        }
    }
}