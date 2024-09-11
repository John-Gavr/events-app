using Events.Application.DTOs.Participants.Requests.GetParticipantById;
using Events.Application.DTOs.Participants.Requests.GetParticipantsByEventId;
using Events.Application.DTOs.Participants.Requests.RegisterParticipant;
using Events.Application.DTOs.Participants.Requests.UnregisterParticipant;
using Events.Application.DTOs.Participants.Responses;

namespace Events.Application.Interfaces;

public interface IEventParticipantService
{
    Task RegisterParticipantAsync(RegisterParticipantRequest request);
    Task<IEnumerable<EventParticipantResponse>> GetParticipantsByEventIdAsync(GetParticipantsByEventIdRequest request);
    Task<EventParticipantResponse?> GetParticipantByIdAsync(GetParticipantByIdRequest request);
    Task UnregisterParticipantAsync(UnregisterParticipantRequest request);
}
