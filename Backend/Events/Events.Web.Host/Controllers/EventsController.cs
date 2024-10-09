using Events.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Events.Application.DTOs.Events.Requests;
using Events.Application.DTOs.Events.Responces;
using Events.Application.DTOs.Events.Requests.CreateEvent;
using Events.Application.DTOs.Events.Requests.DeleteEvent;
using Events.Application.DTOs.Events.Requests.GetAllEvents;
using Events.Application.DTOs.Events.Requests.GetEventByName;
using Events.Application.DTOs.Events.Requests.UpdateEvent;
using Events.Application.DTOs.Events.Requests.UpdateEventsImage;
using Events.Application.DTOs.Events.Requests.GetUsersEvents;

namespace Events.Web.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<EventsResponse>> GetAllEventsAsync([FromQuery] GetAllEventsRequest request, CancellationToken cancellationToken)
    {
        var eventsResponse = await _eventService.GetAllEventsAsync(request.PageNumber, request.PageSize, cancellationToken);
        return Ok(eventsResponse);
    }

    [HttpGet("event")]
    [Authorize]
    public async Task<ActionResult<EventResponse>> GetEventByIdAsync([FromQuery] GetEventByIdRequest request, CancellationToken cancellationToken)
    {
        var eventDto = await _eventService.GetEventByIdAsync(request.Id, cancellationToken);
        if (eventDto == null)
        {
            return NotFound();
        }

        return Ok(eventDto);
    }

    [HttpGet("byname")]
    [Authorize]
    public async Task<ActionResult<EventResponse>> GetEventByNameAsync([FromQuery] GetEventByNameRequest request, CancellationToken cancellationToken)
    {
        var eventDto = await _eventService.GetEventByNameAsync(request.Name, cancellationToken);
        if (eventDto == null)
        {
            return NotFound();
        }

        return Ok(eventDto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> CreateEventAsync([FromBody] CreateEventRequest request, CancellationToken cancellationToken)
    {
        await _eventService.AddEventAsync(request, cancellationToken);
        return Ok();
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateEventAsync([FromQuery] int id, [FromBody] UpdateEventRequest request, CancellationToken cancellationToken)
    {
        await _eventService.UpdateEventAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteEventAsync([FromQuery] DeleteEventRequest request, CancellationToken cancellationToken)
    {
        await _eventService.DeleteEventAsync(request.Id, cancellationToken);
        return NoContent();
    }

    [HttpGet("bycriteria")]
    [Authorize]
    public async Task<ActionResult<EventsResponse>> GetEventsByCriteriaAsync([FromQuery] GetEventsByCriteriaRequest request, CancellationToken cancellationToken)
    {
        var events = await _eventService.GetEventsByCriteriaAsync(cancellationToken, request.Date, request.Location, request.Category, request.PageNumber, request.PageSize);
        return Ok(events);
    }

    [HttpPost("addImage")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateEventsImageAsync([FromBody] UpdateEventImageRequest request, CancellationToken cancellationToken)
    {
        await _eventService.UpdateEventsImageAsync(request, cancellationToken);
        return NoContent();
    }

    [HttpGet("userEvents")]
    [Authorize]
    public async Task<EventsResponse> GetEventsByUserId([FromQuery] GetUsersEventsRequest request, CancellationToken cancellationToken)
    {
        return await _eventService.GetEventsByUserIdAsync(request.UserId, request.PageNumber, request.PageSize, cancellationToken);
    }
}
