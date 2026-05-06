using MediatR;
using IdentityHub.BFF.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.User;
using Microsoft.AspNetCore.Authentication;

namespace IdentityHub.BFF.Features.Profile.Update.UpdateUserName
{
    public static class UpdateUserNameEndpoint
    {
        public static void MapUpdateUserName(this IEndpointRouteBuilder app)
        {
            app.MapPatch("update-name", async (HttpContext httpContext, [FromBody] UpdateUserNameRequest request, [FromServices] JwtReader jwtReader, [FromServices] IMediator mediator) =>
            {
                var token = await httpContext.GetTokenAsync("access_token");

                var jwtReaderDTO = jwtReader.Extract(token!);

                var command = new UpdateUserNameCommand(jwtReaderDTO.UserId, request.UserName);

                var result = await mediator.Send(command);

                return Results.Ok();
            }).RequireAuthorization();
        }
    }
}