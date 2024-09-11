namespace Events.Application.DTOs.Participants.Requests.GetParticipantsByEventId;

public class GetParticipantsByEventIdRequest
{
    public int EventId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
