using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Events.Application.UseCases.Participants.Queries.GetParticipantByUserId;
using Events.Application.UseCases.Participants.Queries.GetParticipantsByEventId;
using Events.Application.UseCases.Participants.Commands.RegisterParticipant;
using Events.Application.UseCases.Participants.Commands.UnregisterParticipant;
using MediatR;

namespace Events.Web.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventParticipantController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EventParticipantController(IHttpContextAccessor httpContextAccessor, IMediator mediator)
    {       
        _httpContextAccessor = httpContextAccessor;
        _mediator = mediator;
    }

    [HttpPost("register")]
    [Authorize]
    public async Task<IActionResult> RegisterParticipantAsync([FromBody] RegisterParticipantCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        request.UserId = userId!;

        await _mediator.Send(request, cancellationToken);
        return Ok(new { Message = "Participant registered successfully." });
    }

    [HttpGet("event/participants")]
    [Authorize]
    public async Task<IActionResult> GetParticipantsByEventIdAsync([FromQuery] GetParticipantsByEventIdQuery request, CancellationToken cancellationToken)
    {
        var participants = await _mediator.Send(request, cancellationToken);
        return Ok(participants);
    }

    [HttpGet("participant")]
    [Authorize]
    public async Task<IActionResult> GetParticipantByUserIdAsync([FromQuery] GetParticipantByUserIdQuery request, CancellationToken cancellationToken)
    {
        var participant = await _mediator.Send(request, cancellationToken);
        return Ok(participant);
    }

    [HttpDelete("event/unregister")]
    [Authorize]
    public async Task<IActionResult> UnregisterParticipantAsync([FromQuery] UnregisterParticipantCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        request.UserId = userId!;

        await _mediator.Send(request, cancellationToken);
        return Ok(new { Message = "The user's participation has been canceled." });
    }
}
