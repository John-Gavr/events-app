using Microsoft.AspNetCore.Mvc;
using Events.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Events.Application.DTOs.Roles.Requests;
using System.Threading;

namespace Events.Web.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRolesService _rolesService;

    public RolesController(IRolesService rolesService)
    {
        _rolesService = rolesService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAllRoles(CancellationToken cancellationToken)
    {
        var roles = _rolesService.GetAllRoles(cancellationToken);
        return Ok(roles);
    }

    [HttpGet("users/user/roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserRolesAsync([FromQuery] GetUserRolesRequest request, CancellationToken cancellationToken)
    {
        var roles = await _rolesService.GetUsersRoleAsync(request, cancellationToken);
        return Ok(roles);
    }

    [HttpPost("users/user/roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SetUserRoleAsync(SetUsersRolesRequest request, CancellationToken cancellationToken)
    {
        await _rolesService.SetUsersRoleAsync(request, cancellationToken);
        return Ok(new { Message = "Role assigned successfully." });
    }
}
