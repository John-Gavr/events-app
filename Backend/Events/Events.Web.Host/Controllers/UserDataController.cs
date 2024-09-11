using Events.Application.DTOs.Users.Requests.GetUserDataByEmail;
using Events.Application.DTOs.Users.Requests.GetUserDataById;
using Events.Application.DTOs.Users.Responses;
using Events.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.Web.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserDataController : ControllerBase
{
    private readonly IUserDataService _userDataService;

    public UserDataController(IUserDataService userDataService) { _userDataService = userDataService; }

    [HttpGet("byEmail")]
    [Authorize(Policy = "CurrentUserByEmailPolicy")]
    public async Task<UserDataResponse> GetUserDataByEmailAsync([FromQuery] GetUserDataByEmailRequest request) {
        return await _userDataService.GetUserDataByEmail(request);
    }
    [HttpGet("byId")]
    [Authorize(Policy = "CurrentUserPolicy")]
    public async Task<UserDataResponse> GetUserDataByIdAsync([FromQuery] GetUserDataByIdRequest request) {
        return await _userDataService.GetUserDataById(request);
    }
}
