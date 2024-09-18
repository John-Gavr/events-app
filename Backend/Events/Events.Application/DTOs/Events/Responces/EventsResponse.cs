namespace Events.Application.DTOs.Events.Responces;

public class EventsResponse
{
    public IEnumerable<EventResponse> Events { get; set; } = [];
    public int TotalCount { get; set; }

}
