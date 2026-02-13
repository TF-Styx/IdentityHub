using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.SRP;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace IdentityHub.BFF.Features.Auth.SRPVerify
{
    public static class SRPVerifyEndpoint
    {
        public static void MapSRPVerify(this IEndpointRouteBuilder app)
        {
            app.MapPost("auth/srp/verify", async (HttpContext context, [FromBody] SRPVerifyRequest request, [FromServices] IMediator mediator, CancellationToken cancellationToken = default) =>
            {
                var command = new SRPVerifyCommand(request.Login, request.A, request.M1);

                var result = await mediator.Send(command, cancellationToken);

                if (result.IsFailure)
                    return Results.BadRequest(result.Errors);

                var tokens = result.Value;

                var claims = new List<Claim> { new (ClaimTypes.Name, request.Login) };

                ClaimsIdentity claimsIdentity = new (claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties authProp = new ();

                authProp.StoreTokens
                    (
                        [
                            new AuthenticationToken { Name = "access_token", Value = tokens.AccessToken }, 
                            new AuthenticationToken { Name = "refresh_token", Value = tokens.RefreshToken }
                        ]
                    );

                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProp);

                return Results.Ok(result.Value);
            });
        }
    }
}