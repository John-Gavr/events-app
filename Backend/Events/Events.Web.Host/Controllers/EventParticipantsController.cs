using Microsoft.AspNetCore.Mvc;
using Events.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Events.Application.DTOs.Participants.Requests.GetParticipantById;
using Events.Application.DTOs.Participants.Requests.GetParticipantsByEventId;
using Events.Application.DTOs.Participants.Requests.RegisterParticipant;
using Events.Application.DTOs.Participants.Requests.UnregisterParticipant;
using System.Security.Claims;

namespace Events.Web.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventParticipantController : ControllerBase
{
    private readonly IEventParticipantService _eventParticipantService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EventParticipantController(IEventParticipantService eventParticipantService, IHttpContextAccessor httpContextAccessor)
    {
        _eventParticipantService = eventParticipantService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("register")]
    [Authorize]
    public async Task<IActionResult> RegisterParticipantAsync([FromBody] RegisterParticipantRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _eventParticipantService.RegisterParticipantAsync(request, userId!, cancellationToken);
        return Ok(new { Message = "Participant registered successfully." });
    }

    [HttpGet("event/participants")]
    [Authorize]
    public async Task<IActionResult> GetParticipantsByEventIdAsync([FromQuery] GetParticipantsByEventIdRequest request, CancellationToken cancellationToken)
    {
        var participants = await _eventParticipantService.GetParticipantsByEventIdAsync(request, cancellationToken);
        return Ok(participants);
    }

    [HttpGet("participant")]
    [Authorize]
    public async Task<IActionResult> GetParticipantByUserIdAsync([FromQuery] GetParticipantByUserIdRequest request, CancellationToken cancellationToken)
    {
        var participant = await _eventParticipantService.GetParticipantByUserIdAsync(request, cancellationToken);
        return Ok(participant);
    }

    [HttpDelete("event/unregister")]
    public async Task<IActionResult> UnregisterParticipantAsync([FromQuery] UnregisterParticipantRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _eventParticipantService.UnregisterParticipantAsync(request, userId!, cancellationToken);
        return Ok(new { Message = "The user's participation has been canceled." });
    }
}
