using MediatR;

namespace Events.Application.UseCases.Events.Commands.CreateEvent;
public class CreateEventCommand : IRequest<Unit>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDateTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int MaxParticipants { get; set; }
    public string Image { get; set; } = string.Empty;
}
