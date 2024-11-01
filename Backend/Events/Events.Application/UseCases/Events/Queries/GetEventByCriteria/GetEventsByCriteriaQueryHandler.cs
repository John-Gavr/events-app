using AutoMapper;
using Events.Application.UseCases.Events.DTOs;
using Events.Core.Interfaces;
using MediatR;

namespace Events.Application.UseCases.Events.Queries.GetEventByCriteria;

public class GetEventsByCriteriaQueryHandler : EventsQueryHandlerBase, IRequestHandler<GetEventsByCriteriaQuery, EventsResponseDTO>
{
    public GetEventsByCriteriaQueryHandler(IMapper mapper, IEventRepository eventRepository) : base(mapper, eventRepository)
    { }

    public async Task<EventsResponseDTO> Handle(GetEventsByCriteriaQuery query, CancellationToken cancellationToken)
    {
        var events = await _eventRepository.GetEventsByCriteriaAsync(cancellationToken, query.Date,
            query.Location, query.Category, query.PageNumber, query.PageSize);
        int totalCount = await _eventRepository.GetNumberOfAllEventsByCriteriaAsync(cancellationToken, query.Date, query.Location, query.Category);

        return new EventsResponseDTO
        {
            Events = _mapper.Map<IEnumerable<EventResponseDTO>>(events),
            TotalCount = totalCount
        };
    }
}
