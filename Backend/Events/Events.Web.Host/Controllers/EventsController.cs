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
    public async Task<ActionResult<IEnumerable<EventResponse>>> GetAllEventsAsync([FromQuery] GetAllEventsRequest request)
    {
        var events = await _eventService.GetAllEventsAsync(request.PageNumber, request.PageSize);
        return Ok(events);
    }

    [HttpGet("event")]
    [Authorize]
    public async Task<ActionResult<EventResponse>> GetEventByIdAsync([FromQuery] GetEventByIdRequest request)
    {
        var eventDto = await _eventService.GetEventByIdAsync(request.Id);
        if (eventDto == null)
        {
            return NotFound();
        }

        return Ok(eventDto);
    }

    [HttpGet("byname")]
    [Authorize]
    public async Task<ActionResult<EventResponse>> GetEventByNameAsync([FromQuery] GetEventByNameRequest request)
    {
        var eventDto = await _eventService.GetEventByNameAsync(request.Name);
        if (eventDto == null)
        {
            return NotFound();
        }

        return Ok(eventDto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> CreateEventAsync([FromBody] CreateEventRequest request)
    {
        await _eventService.AddEventAsync(request);
        return CreatedAtAction(nameof(GetEventByNameAsync), new { name = request.Name }, request); Ok();
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateEventAsync([FromQuery] int id, [FromBody] UpdateEventRequest request)
    {
        await _eventService.UpdateEventAsync(id, request);
        return NoContent();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteEventAsync([FromQuery] DeleteEventRequest request)
    {
        await _eventService.DeleteEventAsync(request.Id);
        return NoContent();
    }

    [HttpGet("bycriteria")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<EventResponse>>> GetEventsByCriteriaAsync([FromQuery] GetEventsByCriteriaRequest request)
    {
        var events = await _eventService.GetEventsByCriteriaAsync(request.Date, request.Location, request.Category, request.PageNumber, request.PageSize);
        return Ok(events);
    }

    [HttpPost("addImage")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateEventsImageAsync([FromBody] UpdateEventImageRequest request)
    {
        await _eventService.UpdateEventsImageAsync(request);
        return NoContent();
    }
}
