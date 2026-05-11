using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.User;
using Microsoft.AspNetCore.Authorization;
using IdentityHub.IdentityService.Application.Features.Users.Register;
using IdentityHub.IdentityService.Application.Features.Users.GenerateConfirmCode;
using IdentityHub.IdentityService.Application.Features.Users.VerifyConfirmCode;
using IdentityHub.IdentityService.Application.Features.Users.RecoveryAccess;
using IdentityHub.IdentityService.Application.Features.Users.GetGeneralInfo;
using IdentityHub.IdentityService.Application.Features.Users.UpdateUserName;
using IdentityHub.IdentityService.Application.Features.Users.UploadAvatar;

namespace IdentityHub.IdentityService.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("general-info/{userId}")]
        public async Task<IActionResult> GetGeneralInfo([FromRoute] Guid userId, CancellationToken cancellationToken = default)
        {
            var command = new GetGeneralInfoQuery(userId);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(result.Value),
                    onFailure: errors => BadRequest(errors)
                );
        }

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

        [HttpPost("generate-recovery-access-code/{login}")]
        public async Task<IActionResult> GenerateCode([FromRoute] string login, CancellationToken cancellationToken = default)
        {
            var command = new GenerateConfirmCodeCommand(login);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpPost("verify-confirm-code/{login}/{code}")]
        public async Task<IActionResult> VerifyConfirmCode([FromRoute] string login, [FromRoute] int code, CancellationToken cancellationToken = default)
        {
            var command = new VerifyConfirmCodeCommand(login, code);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpPatch("recovery-access-password")]
        public async Task<IActionResult> RecoveryAccessPassword([FromBody] RecoveryAccessPasswordRequest request, CancellationToken cancellationToken = default)
        {
            var command = new RecoveryAccessCommand
                (
                    request.Login, 
                    request.Verifier,
                    request.ClientSalt,
                    request.EncryptedDek,
                    request.EncryptionAlgorithm,
                    request.Iterations,
                    request.KdfType
                );  

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpPatch("update-name")]
        public async Task<IActionResult> UpdateUserName([FromBody] UpdateUserNameRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UpdateUserNameCommand(Guid.Parse(request.UserId), request.UserName);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpPatch("upload-avatar")]
        public async Task<IActionResult> UploadAvatar([FromBody] UploadAvatarRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UploadAvatarCommand(Guid.Parse(request.UserId), request.BucketName, request.FileName);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}
