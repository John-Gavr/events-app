using AutoMapper;
using Events.Core.Interfaces;

namespace Events.Application.UseCases.Events.Commands;

public class EventsCommandHandlerBase
{
    protected readonly IEventRepository _eventRepository;
    protected readonly IMapper _mapper;

    public EventsCommandHandlerBase(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }
}
