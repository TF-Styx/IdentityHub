namespace IdentityHub.BFF.Features.Auth.CheckAuthStatus
{
    public static class CheckAuthStatusEndpoint
    {
        public static void MapCheckAuthStatus(this IEndpointRouteBuilder app) 
            => app.MapGet("auth/status", () => Results.Ok(new { isAuthenticated = true })).RequireAuthorization();
    }
}