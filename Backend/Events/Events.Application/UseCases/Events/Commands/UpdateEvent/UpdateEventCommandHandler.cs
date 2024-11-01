using AutoMapper;
using Events.Core.Entities.Exceptions;
using Events.Core.Interfaces;
using MediatR;

namespace Events.Application.UseCases.Events.Commands.UpdateEvent;

public class UpdateEventCommandHandler : EventsCommandHandlerBase, IRequestHandler<UpdateEventCommand, Unit>
{
    public UpdateEventCommandHandler(IEventRepository eventRepository, IMapper mapper) : base(eventRepository, mapper)
    { }

    public async Task<Unit> Handle(UpdateEventCommand command, CancellationToken cancellationToken)
    {
        var eventToUpdate = await _eventRepository.GetEventByIdAsync(command.Id, cancellationToken);

        if (eventToUpdate == null)
            throw new NotFoundException(nameof(eventToUpdate), command.Id);

        _mapper.Map(command, eventToUpdate);
        await _eventRepository.UpdateEventAsync(eventToUpdate, cancellationToken);

        return Unit.Value;
    }
}
