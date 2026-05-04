using MediatR;
using IdentityHub.BFF.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

namespace IdentityHub.BFF.Features.Profile
{
    public static class GetGeneralInfoEndpoint
    {
        public static void MapGetGeneralInfo(this IEndpointRouteBuilder app)
        {
            app.MapGet("general-info", async (HttpContext httpContext, [FromServices] JwtReader jwtReader, [FromServices] IMediator mediator) =>
            {
                var token = await httpContext.GetTokenAsync("access_token");

                var jwtReaderDTO = jwtReader.Extract(token!);

                var command = new GetGeneralInfoQuery(jwtReaderDTO.UserId); 
                
                var result = await mediator.Send(command);

                return Results.Ok(result.Value);
            }).RequireAuthorization();
        }
    }
}