using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.User;
using Microsoft.AspNetCore.Authorization;
using IdentityHub.IdentityService.Application.Features.Users.Register;

namespace IdentityHub.IdentityService.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand
                (
                    request.Login, 
                    request.UserName,
                    request.Verifier,
                    request.ClientSalt,
                    request.EncryptedDek,
                    request.EncryptionAlgorithm,
                    request.Iterations,
                    request.KdfType,
                    request.Email,
                    request.Phone
                );

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}
