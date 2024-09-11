namespace Events.Application.DTOs.Participants.Requests.UnregisterParticipant;

public class UnregisterParticipantRequest
{
    public int EventId { get; set; }
    public int ParticipantId { get; set; }
}
