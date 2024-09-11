namespace Events.Application.DTOs.Events.Requests.GetAllEvents;

public class GetAllEventsRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
