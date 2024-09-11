using Events.Core.Entities;

namespace Events.Core.Interfaces;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetAllEventsAsync(int pageNumber, int pageSize);
    Task<Event?> GetEventByIdAsync(int id);
    Task<Event?> GetEventByNameAsync(string name);
    Task AddEventAsync(Event newEvent);
    Task UpdateEventAsync(Event updatedEvent);
    Task DeleteEventAsync(int id);
    Task<IEnumerable<Event>> GetEventsByCriteriaAsync(DateTime? date = null, string? location = null, string? category = null, int pageNumber = 1, int pageSize = 10);
    public  Task AddEventImageAsync(int id, byte[] image);
}
