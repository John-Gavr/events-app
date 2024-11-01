using AutoMapper;
using Events.Application.UseCases.Events.DTOs;
using Events.Core.Entities.Exceptions;
using Events.Core.Interfaces;
using MediatR;

namespace Events.Application.UseCases.Events.Queries.GetEventByName;

public class GetEventByNameQueryHandler : EventsQueryHandlerBase, IRequestHandler<GetEventByNameQuery, EventResponseDTO>
{
    public GetEventByNameQueryHandler(IMapper mapper, IEventRepository eventRepository) : base(mapper, eventRepository)
    { }

    public async Task<EventResponseDTO> Handle(GetEventByNameQuery query, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetEventByNameAsync(query.Name, cancellationToken);
        if (eventEntity == null)
            throw new NotFoundException(nameof(eventEntity), query.Name);

        return _mapper.Map<EventResponseDTO>(eventEntity);
    }
}
