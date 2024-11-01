using Events.Application.Interfaces;
using Events.Application.UseCases.Users.Queries.GetUserDataById;
using Events.Application.UseCases.Users.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediatR;

namespace Events.Web.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserDataController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserDataController(IHttpContextAccessor httpContextAccessor, IMediator mediator)
    {
        _httpContextAccessor = httpContextAccessor;
        _mediator = mediator;
    }

    [HttpGet("Id")]
    [Authorize]
    public UserIdResponseDTO GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return new UserIdResponseDTO
        {
            Id = userId!
        };
    }

    [HttpGet("byId")]
    [Authorize(Roles = "Admin")]
    public async Task<UserDataResponseDTO> GetUserDataByUserIdAsync([FromQuery] GetUserDataByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _mediator.Send(request, cancellationToken);
    }

    [HttpGet]
    [Authorize]
    public async Task<UserDataResponseDTO> GetUserDataAsync(CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _mediator.Send(new GetUserDataByUserIdQuery
        {
            UserId = userId!
        }, cancellationToken);
    }
}
