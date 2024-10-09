using Events.Application.DTOs.Participants.Requests.GetParticipantById;
using Events.Application.DTOs.Participants.Requests.GetParticipantsByEventId;
using Events.Application.DTOs.Participants.Requests.RegisterParticipant;
using Events.Application.DTOs.Participants.Requests.UnregisterParticipant;
using Events.Application.DTOs.Participants.Responses;

namespace Events.Application.Interfaces;

public interface IEventParticipantService
{
    Task RegisterParticipantAsync(RegisterParticipantRequest request, string userId, CancellationToken cancellationToken);
    Task<IEnumerable<EventParticipantResponse>> GetParticipantsByEventIdAsync(GetParticipantsByEventIdRequest request, CancellationToken cancellationToken);
    Task<EventParticipantResponse?> GetParticipantByUserIdAsync(GetParticipantByUserIdRequest request, CancellationToken cancellationToken);
    Task UnregisterParticipantAsync(UnregisterParticipantRequest request, string userId, CancellationToken cancellationToken);
}
