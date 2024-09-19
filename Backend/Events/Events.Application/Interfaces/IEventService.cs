using Events.Application.DTOs.Events.Requests.CreateEvent;
using Events.Application.DTOs.Events.Requests.UpdateEvent;
using Events.Application.DTOs.Events.Requests.UpdateEventsImage;
using Events.Application.DTOs.Events.Responces;

namespace Events.Application.Interfaces;

public interface IEventService
{
    Task<EventsResponse> GetAllEventsAsync(int pageNumber, int pageSize);
    Task<EventResponse?> GetEventByIdAsync(int id);
    Task<EventResponse?> GetEventByNameAsync(string name);
    Task AddEventAsync(CreateEventRequest request);
    Task UpdateEventAsync(int id, UpdateEventRequest request);
    Task DeleteEventAsync(int id);
    Task<EventsResponse> GetEventsByCriteriaAsync(DateTime? date = null, string? location = null, string? category = null, int pageNumber = 1, int pageSize = 10);
    Task UpdateEventsImageAsync(UpdateEventImageRequest request);
    Task<EventsResponse> GetEventsByUserIdAsync(string userId, int pageNumber, int pageSize);
}
