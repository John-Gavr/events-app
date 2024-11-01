using Events.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Events.Application.UseCases.Events.DTOs;
using Events.Application.UseCases.Events.Commands.CreateEvent;
using Events.Application.UseCases.Events.Commands.DeleteEvent;
using Events.Application.UseCases.Events.Commands.UpdateEvent;
using Events.Application.UseCases.Events.Commands.UpdateEventsImage;
using Events.Application.UseCases.Events.Queries.GetAllEvents;
using Events.Application.UseCases.Events.Queries.GetEventByCriteria;
using Events.Application.UseCases.Events.Queries.GetEventById;
using Events.Application.UseCases.Events.Queries.GetEventByName;
using Events.Application.UseCases.Events.Queries.GetUsersEvents;
using MediatR;

namespace Events.Web.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;
    public EventsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<EventsResponseDTO>> GetAllEventsAsync([FromQuery] GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    [HttpGet("event")]
    [Authorize]
    public async Task<ActionResult<EventResponseDTO>> GetEventByIdAsync([FromQuery] GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    [HttpGet("byname")]
    [Authorize]
    public async Task<ActionResult<EventResponseDTO>> GetEventByNameAsync([FromQuery] GetEventByNameQuery request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> CreateEventAsync([FromBody] CreateEventCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);
        return Ok();
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateEventAsync([FromQuery] int id, [FromBody] UpdateEventCommand request, CancellationToken cancellationToken)
    {
        request.Id = id;
        await _mediator.Send(request, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteEventAsync([FromQuery] DeleteEventCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request.Id, cancellationToken);
        return NoContent();
    }

    [HttpGet("bycriteria")]
    [Authorize]
    public async Task<ActionResult<EventsResponseDTO>> GetEventsByCriteriaAsync([FromQuery] GetEventsByCriteriaQuery request, CancellationToken cancellationToken)
    {
        var events = await _mediator.Send(request, cancellationToken);
        return Ok(events);
    }

    [HttpPost("addImage")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateEventsImageAsync([FromBody] UpdateEventImageCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);
        return NoContent();
    }

    [HttpGet("userEvents")]
    [Authorize]
    public async Task<EventsResponseDTO> GetEventsByUserId([FromQuery] GetUsersEventsQuery request, CancellationToken cancellationToken)
    {
        return await _mediator.Send(request, cancellationToken);
    }
}
