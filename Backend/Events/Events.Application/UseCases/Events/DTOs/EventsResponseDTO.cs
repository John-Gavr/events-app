namespace Events.Application.UseCases.Events.DTOs;

public class EventsResponseDTO
{
    public IEnumerable<EventResponseDTO> Events { get; set; } = [];
    public int TotalCount { get; set; }

}
