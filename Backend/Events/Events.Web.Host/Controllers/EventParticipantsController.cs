using Microsoft.AspNetCore.Mvc;
using Events.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Events.Application.DTOs.Participants.Requests.GetParticipantById;
using Events.Application.DTOs.Participants.Requests.GetParticipantsByEventId;
using Events.Application.DTOs.Participants.Requests.RegisterParticipant;
using Events.Application.DTOs.Participants.Requests.UnregisterParticipant;

namespace Events.Web.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventParticipantController : ControllerBase
{
    private readonly IEventParticipantService _eventParticipantService;

    public EventParticipantController(IEventParticipantService eventParticipantService)
    {
        _eventParticipantService = eventParticipantService;
    }

    
    [HttpPost("register")]
    [Authorize]
    public async Task<IActionResult> RegisterParticipant([FromBody] RegisterParticipantRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _eventParticipantService.RegisterParticipantAsync(request);
        return Ok(new { Message = "Participant registred successfully." });
    }

    [HttpGet("event/participants")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetParticipantsByEventId([FromQuery] GetParticipantsByEventIdRequest request)
    {
        var participants = await _eventParticipantService.GetParticipantsByEventIdAsync(request);
        return Ok(participants);
    }

    [HttpGet("participant")]
    [Authorize]
    public async Task<IActionResult> GetParticipantById(GetParticipantByIdRequest request)
    {
        var participant = await _eventParticipantService.GetParticipantByIdAsync(request);
        return Ok(participant);
    }

    [HttpDelete("event/unregister")]
    [Authorize(Policy = "ParticipantPolicy")]
    public async Task<IActionResult> UnregisterParticipant([FromQuery] UnregisterParticipantRequest request)
    {
        await _eventParticipantService.UnregisterParticipantAsync(request);
        return Ok(new { Message = "The user's participation has been canceled." });
    }
}
