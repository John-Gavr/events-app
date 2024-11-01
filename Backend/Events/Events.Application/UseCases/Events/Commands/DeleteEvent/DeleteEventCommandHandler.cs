using AutoMapper;
using Events.Core.Entities.Exceptions;
using Events.Core.Interfaces;
using MediatR;

namespace Events.Application.UseCases.Events.Commands.DeleteEvent
{
    public class DeleteEventCommandHandler : EventsCommandHandlerBase, IRequestHandler<DeleteEventCommand, Unit>
    {
        public DeleteEventCommandHandler(IEventRepository eventRepository, IMapper mapper) : base(eventRepository, mapper)
        { }

        public async Task<Unit> Handle(DeleteEventCommand command, CancellationToken cancellationToken)
        {
            var eventEntity = await _eventRepository.GetEventByIdAsync(command.Id, cancellationToken);
            if (eventEntity == null)
                throw new NotFoundException(nameof(eventEntity), command.Id);

            await _eventRepository.DeleteEventAsync(command.Id, cancellationToken);
            return Unit.Value;
        }
    }
}
