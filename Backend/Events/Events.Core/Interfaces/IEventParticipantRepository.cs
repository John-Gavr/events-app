using Events.Core.Entities;

namespace Events.Core.Interfaces;

public interface IEventParticipantRepository
{
    Task RegisterParticipantAsync(int eventId, EventParticipant participant);
    Task<IEnumerable<EventParticipant>> GetParticipantsByEventIdAsync(int eventId, int pageNumber, int pageSize);
    Task<EventParticipant?> GetParticipantByIdAsync(int participantId);
    Task UnregisterParticipantAsync(int eventId, int participantId);
}
