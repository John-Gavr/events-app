using AutoMapper;
using Events.Core.Interfaces;

namespace Events.Application.UseCases.Events.Queries;

public class EventsQueryHandlerBase
{
    protected readonly IEventRepository _eventRepository;
    protected readonly IMapper _mapper;

    public EventsQueryHandlerBase(IMapper mapper, IEventRepository eventRepository)
    {
        _mapper = mapper;
        _eventRepository = eventRepository;
    }
}
