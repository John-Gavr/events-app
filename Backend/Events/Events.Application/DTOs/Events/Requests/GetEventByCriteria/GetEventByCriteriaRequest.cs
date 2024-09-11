namespace Events.Application.DTOs.Events.Requests;

public class GetEventsByCriteriaRequest
{
    public DateTime? Date { get; set; }
    public string? Location { get; set; }
    public string? Category { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
