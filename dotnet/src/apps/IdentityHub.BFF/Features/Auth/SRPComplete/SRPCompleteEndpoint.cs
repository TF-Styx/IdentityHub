using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace IdentityHub.BFF.Features.Auth.SRPComplete
{
    public static class SRPCompleteEndpoint
    {
        public static void MapSRPComplete(this IEndpointRouteBuilder app)
        {
            app.MapPost("auth/srp/complete", async (HttpContext context, [FromBody] SRPCompleteRequest request, [FromServices] IMediator mediator, CancellationToken cancellationToken = default) =>
            {
                var command = new SRPCompleteCommand(request.TempAuthToken);

                var result = await mediator.Send(command, cancellationToken);

                if (result.IsFailure)
                    return Results.InternalServerError(result.StringMessage);

                var userSession = result.Value;

                List<Claim> claims =
                [
                    new (ClaimTypes.NameIdentifier, userSession.UserId),
                    new (ClaimTypes.Name, userSession.Login),
                    new ("SessionId", userSession.SessionId) 
                ];

                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(claimIdentity);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(30),
                    AllowRefresh = true
                };

                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                return Results.Ok();
            });
        }
    }
}