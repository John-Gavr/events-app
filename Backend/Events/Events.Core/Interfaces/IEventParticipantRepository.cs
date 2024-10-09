using Events.Core.Entities;

namespace Events.Core.Interfaces;

public interface IEventParticipantRepository
{
    Task RegisterParticipantAsync(int eventId, EventParticipant participant, CancellationToken cancellationToken);
    Task<IEnumerable<EventParticipant>> GetParticipantsByEventIdAsync(int eventId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<EventParticipant?> GetParticipantByUserIdAsync(string userId, CancellationToken cancellationToken);
    Task UnregisterParticipantAsync(int eventId, string userId, CancellationToken cancellationToken);
}
