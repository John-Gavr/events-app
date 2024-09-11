namespace Events.Application.DTOs.Participants.Responses;

public class EventParticipantResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public int EventId { get; set; }
}

