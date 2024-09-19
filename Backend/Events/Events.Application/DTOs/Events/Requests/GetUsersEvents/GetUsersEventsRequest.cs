namespace Events.Application.DTOs.Events.Requests.GetUsersEvents;

public class GetUsersEventsRequest
{
    public string UserId { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
