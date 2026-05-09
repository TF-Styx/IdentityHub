using MediatR;
using IdentityHub.BFF.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Diagnostics;

namespace IdentityHub.BFF.Features.Profile.Avatar
{
    public static class AvatarEndpoint
    {
        public static void MapAvatar(this IEndpointRouteBuilder app)
        {
            app.MapPost("avatar", async (HttpContext httpContext, [FromForm] IFormFile file, [FromServices] JwtReader jwtReader, [FromServices] IMediator mediator) =>
            {
                Console.WriteLine("Дошел до Endpoint");

                var token = await httpContext.GetTokenAsync("access_token");
                var jwtReaderDTO = jwtReader.Extract(token!);

                var fileStream = file.OpenReadStream();

                var command = new AvatarCommand(jwtReaderDTO.UserId, fileStream, file.FileName, file.ContentType);

                var result = await mediator.Send(command);

                return Results.Ok();
            }).DisableAntiforgery().RequireAuthorization();
        }
    }
}