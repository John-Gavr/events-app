using AutoMapper;
using Events.Core.Entities.Exceptions;
using Events.Core.Entities;
using Events.Core.Interfaces;
using MediatR;

namespace Events.Application.UseCases.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : EventsCommandHandlerBase, IRequestHandler<CreateEventCommand, Unit>
{
    public CreateEventCommandHandler(IEventRepository eventRepository, IMapper mapper) : base(eventRepository, mapper)
    { }

    public async Task<Unit> Handle(CreateEventCommand command, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetEventByNameAsync(command.Name, cancellationToken);
        if (eventEntity != null)
            throw new EventAlredyExistException(command.Name);

        var newEvent = _mapper.Map<Event>(command);
        await _eventRepository.AddEventAsync(newEvent, cancellationToken);
        return Unit.Value;
    }
}
