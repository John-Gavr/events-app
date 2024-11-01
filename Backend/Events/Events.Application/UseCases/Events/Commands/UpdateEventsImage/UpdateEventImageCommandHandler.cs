using AutoMapper;
using Events.Core.Entities.Exceptions;
using Events.Core.Interfaces;
using MediatR;

namespace Events.Application.UseCases.Events.Commands.UpdateEventsImage;

public class UpdateEventImageCommandHandler : EventsCommandHandlerBase, IRequestHandler<UpdateEventImageCommand, Unit>
{
    public UpdateEventImageCommandHandler(IEventRepository eventRepository, IMapper mapper) : base(eventRepository, mapper)
    { }

    public async Task<Unit> Handle(UpdateEventImageCommand command, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetEventByIdAsync(command.EventId, cancellationToken);
        if (eventEntity == null)
            throw new NotFoundException(nameof(eventEntity), command.EventId);

        await _eventRepository.AddEventImageAsync(command.EventId, command.ImageBytes, cancellationToken);
        return Unit.Value;
    }
}
