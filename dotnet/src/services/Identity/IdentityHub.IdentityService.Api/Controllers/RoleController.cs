using IdentityHub.IdentityService.Application.Features.Roles.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.Role;

namespace IdentityHub.IdentityService.Api.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public sealed class RoleController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateRoleCommand(request.Name);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}
