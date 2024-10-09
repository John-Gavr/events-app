using Events.Application.DTOs.Events.Requests.CreateEvent;
using Events.Application.DTOs.Events.Requests.UpdateEvent;
using Events.Application.DTOs.Events.Requests.UpdateEventsImage;
using Events.Application.DTOs.Events.Responces;

namespace Events.Application.Interfaces;

public interface IEventService
{
    Task<EventsResponse> GetAllEventsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<EventResponse?> GetEventByIdAsync(int id, CancellationToken cancellationToken);
    Task<EventResponse?> GetEventByNameAsync(string name, CancellationToken cancellationToken);
    Task AddEventAsync(CreateEventRequest request, CancellationToken cancellationToken);
    Task UpdateEventAsync(int id, UpdateEventRequest request, CancellationToken cancellationToken);
    Task DeleteEventAsync(int id, CancellationToken cancellationToken);
    Task<EventsResponse> GetEventsByCriteriaAsync(CancellationToken cancellationToken, DateTime? date = null, string? location = null, string? category = null, int pageNumber = 1, int pageSize = 10);
    Task UpdateEventsImageAsync(UpdateEventImageRequest request, CancellationToken cancellationToken);
    Task<EventsResponse> GetEventsByUserIdAsync(string userId, int pageNumber, int pageSize, CancellationToken cancellationToken);
}
