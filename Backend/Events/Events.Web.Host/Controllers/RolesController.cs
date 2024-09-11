using Microsoft.AspNetCore.Mvc;
using Events.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Events.Application.DTOs.Roles.Requests;

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
    public IActionResult GetAllRoles()
    {
        var roles = _rolesService.GetAllRoles();
        return Ok(roles);
    }

    [HttpGet("users/user/roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserRoles([FromQuery] GetUserRolesRequest request)
    {
        var roles = await _rolesService.GetUsersRoleAsync(request);
        return Ok(roles);
    }

    [HttpPost("users/user/roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SetUserRole(SetUsersRolesRequest request)
    {
        await _rolesService.SetUsersRoleAsync(request);
        return Ok(new { Message = "Role assigned successfully." });
    }
}
