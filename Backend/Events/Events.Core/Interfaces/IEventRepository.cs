using Events.Core.Entities;

namespace Events.Core.Interfaces;

public interface IEventRepository
{
    public Task<int> GetNumberOfAllEventsAsync(CancellationToken cancellationToken);
    public Task<int> GetNumberOfAllEventsByCriteriaAsync(CancellationToken cancellationToken, DateTime? date = null, string? location = null, string? category = null);
    Task<IEnumerable<Event>> GetAllEventsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<Event?> GetEventByIdAsync(int id, CancellationToken cancellationToken);
    Task<Event?> GetEventByNameAsync(string name, CancellationToken cancellationToken);
    Task AddEventAsync(Event newEvent, CancellationToken cancellationToken);
    Task UpdateEventAsync(Event updatedEvent, CancellationToken cancellationToken);
    Task DeleteEventAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Event>> GetEventsByCriteriaAsync(CancellationToken cancellationToken, DateTime? date = null, string? location = null, string? category = null, int pageNumber = 1, int pageSize = 10);
    public  Task AddEventImageAsync(int id, byte[] image, CancellationToken cancellationToken);
    public Task<IEnumerable<Event>> GetEventsByUserIdAsync(string userId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    public Task<int> GetUserEventsCountAsync(string userId, CancellationToken cancellationToken);
}
