using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Events.Application.UseCases.Roles.Queries.GetUserRoles;
using Events.Application.UseCases.Roles.Commands.SetUsersRoles;
using MediatR;
using Events.Application.UseCases.Roles.Queries.GetAllRoles;

namespace Events.Web.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;
    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllRolesAsync(CancellationToken cancellationToken)
    {
        var roles = await _mediator.Send(new GetAllRolesQuery(), cancellationToken);
        return Ok(roles);
    }

    [HttpGet("users/user/roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserRolesAsync([FromQuery] GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _mediator.Send(request, cancellationToken);
        return Ok(roles);
    }

    [HttpPost("users/user/roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SetUserRoleAsync(SetUsersRolesCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);
        return Ok(new { Message = "Role assigned successfully." });
    }
}
