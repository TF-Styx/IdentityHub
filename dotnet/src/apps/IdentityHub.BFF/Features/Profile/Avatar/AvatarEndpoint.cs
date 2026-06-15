using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace IdentityHub.BFF.Features.Profile.Avatar
{
    public static class AvatarEndpoint
    {
        public static void MapAvatar(this IEndpointRouteBuilder app)
        {
            app.MapPost("avatar", async (HttpContext httpContext, [FromForm] IFormFile file, [FromServices] IMediator mediator) =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                    return Results.Unauthorized();

                var fileStream = file.OpenReadStream();

                var command = new AvatarCommand(userId, fileStream, file.FileName, file.ContentType);

                var result = await mediator.Send(command);

                return Results.Ok();
            }).DisableAntiforgery().RequireAuthorization();
        }
    }
}