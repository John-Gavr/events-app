namespace Events.Application.DTOs.Events.Requests.UpdateEventsImage;

public class UpdateEventImageRequest
{
    public int EventId { get; set; }
    public byte[] ImageBytes { get; set; } = [];
}
