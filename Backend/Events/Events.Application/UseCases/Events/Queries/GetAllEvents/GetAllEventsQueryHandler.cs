using AutoMapper;
using Events.Application.UseCases.Events.DTOs;
using Events.Core.Interfaces;
using MediatR;

namespace Events.Application.UseCases.Events.Queries.GetAllEvents;

public class GetAllEventsQueryHandler : EventsQueryHandlerBase, IRequestHandler<GetAllEventsQuery, EventsResponseDTO>
{
    public GetAllEventsQueryHandler(IEventRepository eventRepository, IMapper mapper) : base(mapper, eventRepository)
    { }

    public async Task<EventsResponseDTO> Handle(GetAllEventsQuery query, CancellationToken cancellationToken)
    {
        var events = await _eventRepository.GetAllEventsAsync(query.PageNumber, query.PageSize, cancellationToken);
        var totalCount = await _eventRepository.GetNumberOfAllEventsAsync(cancellationToken);
        return new EventsResponseDTO()
        {
            Events = _mapper.Map<IEnumerable<EventResponseDTO>>(events),
            TotalCount = totalCount
        };
    }
}
