using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Response.Auth;

namespace IdentityHub.AuthService.Api.Controllers
{
    [ApiController]
    [Route("api/configure")]
    public sealed class CryptoConfigureController(IConfiguration configuration) : Controller
    {
        private readonly IConfiguration _configuration = configuration;

        [HttpGet("public-key")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCryptoConfig()
        {
            var publicKey = _configuration["Security:RSA:PublickKey"];
            return Ok(new 
            {
                PublicKey = publicKey
            });
        }
            // => Ok(new PublicKeyResponse(_configuration["Security:RSA:PublickKey"]!));
    }
}
