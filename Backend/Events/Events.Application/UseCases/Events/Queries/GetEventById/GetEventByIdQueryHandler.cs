using AutoMapper;
using Events.Application.UseCases.Events.DTOs;
using Events.Core.Entities.Exceptions;
using Events.Core.Interfaces;
using MediatR;

namespace Events.Application.UseCases.Events.Queries.GetEventById;

public class GetEventByIdQueryHandler : EventsQueryHandlerBase, IRequestHandler<GetEventByIdQuery, EventResponseDTO>
{
    public GetEventByIdQueryHandler(IEventRepository eventRepository, IMapper mapper) : base(mapper, eventRepository)
    { }

    public async Task<EventResponseDTO> Handle(GetEventByIdQuery query, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetEventByIdAsync(query.Id, cancellationToken);
        if (eventEntity == null)
            throw new NotFoundException(nameof(eventEntity), query.Id);

        return _mapper.Map<EventResponseDTO>(eventEntity);
    }
}
