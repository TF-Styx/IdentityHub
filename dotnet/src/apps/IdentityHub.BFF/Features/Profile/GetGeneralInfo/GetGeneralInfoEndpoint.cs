using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace IdentityHub.BFF.Features.Profile.GetGeneralInfo
{
    public static class GetGeneralInfoEndpoint
    {
        public static void MapGetGeneralInfo(this IEndpointRouteBuilder app)
        {
            app.MapGet("general-info", async (HttpContext httpContext, [FromServices] IMediator mediator) =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                    return Results.Unauthorized();

                var command = new GetGeneralInfoQuery(userId); 
                
                var result = await mediator.Send(command);

                return Results.Ok(result.Value);
            }).RequireAuthorization();
        }
    }
}