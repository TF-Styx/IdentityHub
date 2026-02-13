using IdentityHub.BFF.Clients.Auth;
using Microsoft.AspNetCore.Mvc;

namespace IdentityHub.BFF.Features.PublicKey
{
    public static class GetPublicKeyEndpoitn
    {
        public static void MapPublicKey(this IEndpointRouteBuilder app)
        {
            app.MapGet("get-public-key", async ([FromServices] IAuthService authService, CancellationToken cancellationToken = default) 
                => Results.Ok(await authService.GetCryptoConfig()));
        }
    }
}