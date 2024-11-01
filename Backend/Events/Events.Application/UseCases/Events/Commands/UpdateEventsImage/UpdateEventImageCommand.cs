using MediatR;

namespace Events.Application.UseCases.Events.Commands.UpdateEventsImage;

public class UpdateEventImageCommand : IRequest<Unit>
{
    public int EventId { get; set; }
    public byte[] ImageBytes { get; set; } = [];
}
