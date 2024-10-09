using Events.Application.DTOs.Users.Requests.GetUserDataById;
using Events.Application.DTOs.Users.Responses;
using Events.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Events.Web.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserDataController : ControllerBase
{
    private readonly IUserDataService _userDataService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserDataController(IUserDataService userDataService, IHttpContextAccessor httpContextAccessor)
    {
        _userDataService = userDataService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("Id")]
    [Authorize]
    public UserIdResponse GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return new UserIdResponse
        {
            Id = userId!
        };
    }

    [HttpGet("byId")]
    [Authorize(Roles = "Admin")]
    public async Task<UserDataResponse> GetUserDataByUserIdAsync([FromQuery] GetUserDataByUserIdRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _userDataService.GetUserDataByUserIdAsync(request, cancellationToken);
    }

    [HttpGet]
    [Authorize]
    public async Task<UserDataResponse> GetUserDataAsync(CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _userDataService.GetUserDataAsync(userId, cancellationToken);
    }
}
