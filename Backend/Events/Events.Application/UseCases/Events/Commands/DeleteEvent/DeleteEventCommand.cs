using MediatR;

namespace Events.Application.UseCases.Events.Commands.DeleteEvent;

public class DeleteEventCommand : IRequest<Unit>
{
    public int Id { get; set; }
}
