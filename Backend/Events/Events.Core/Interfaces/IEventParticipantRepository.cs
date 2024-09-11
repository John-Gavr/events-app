using Events.Core.Entities;

namespace Events.Core.Interfaces;

public interface IEventParticipantRepository
{
    Task RegisterParticipantAsync(int eventId, EventParticipant participant);
    Task<IEnumerable<EventParticipant>> GetParticipantsByEventIdAsync(int eventId, int pageNumber, int pageSize);
    Task<EventParticipant?> GetParticipantByUserIdAsync(string userId);
    Task UnregisterParticipantAsync(int eventId, string userId);
}
