using IdentityHub.AuthService.Application.Features.SRPChallenge;
using IdentityHub.AuthService.Application.Features.VerifySRP;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.SRP;

namespace IdentityHub.AuthService.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("srp/challenge")]
        public async Task<IActionResult> SRPChallenge([FromBody] SRPChallengeRequest request, CancellationToken cancellationToken = default)
        {
            var command = new SRPChallengeCommand(request.Login);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    () => Ok(result.Value),
                    errors => BadRequest(new
                    {
                        Title = "Ошибка инициализации входа!",
                        Errors = errors
                    })
                );
        }

        [HttpPost("srp/verify")]
        public async Task<IActionResult> SRPVerify([FromBody] SRPVerifyRequest request, CancellationToken cancellationToken = default)
        {
            var command = new VerifySRPCommand(request.Login, request.A, request.M1);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    authResponse => Ok(authResponse),
                    errors => BadRequest(new
                    {
                        Title = "Ошибка проверки данных входа!",
                        Errors = errors
                    })
                );
        }
    }
}
