using IdentityHub.IdentityService.Application.Features.Users.InternalUser.InternalUserByLogin;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityHub.IdentityService.Api.Controllers
{
    [ApiController]
    [Route("internal/api/users")]
    public class InternalUserController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("by-login/{login}")]
        public async Task<IActionResult> GetUserByLoginAsync([FromRoute] string login, CancellationToken cancellationToken = default)
        {
            var command = new InternalUserByLoginQuery(login);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(result.Value),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}