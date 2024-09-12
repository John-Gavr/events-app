using AutoMapper;
using Events.Application.DTOs.Events.Requests.CreateEvent;
using Events.Application.DTOs.Events.Requests.UpdateEvent;
using Events.Application.DTOs.Events.Requests.UpdateEventsImage;
using Events.Application.DTOs.Events.Responces;
using Events.Application.Interfaces;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Events.Core.Interfaces;

namespace Events.Application.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public EventService(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EventResponse>> GetAllEventsAsync(int pageNumber, int pageSize)
    {
        var events = await _eventRepository.GetAllEventsAsync(pageNumber, pageSize);
        return _mapper.Map<IEnumerable<EventResponse>>(events);
    }

    public async Task<EventResponse?> GetEventByIdAsync(int id)
    {
        var eventEntity = await _eventRepository.GetEventByIdAsync(id);
        if (eventEntity != null)
            throw new NotFoundException(nameof(eventEntity), id);

        return _mapper.Map<EventResponse>(eventEntity);
    }

    public async Task<EventResponse?> GetEventByNameAsync(string name)
    {
        var eventEntity = await _eventRepository.GetEventByNameAsync(name);
        if (eventEntity != null)
            throw new NotFoundException(nameof(eventEntity), name);

        return _mapper.Map<EventResponse>(eventEntity);
    }

    public async Task AddEventAsync(CreateEventRequest request)
    {
        var newEvent = _mapper.Map<Event>(request);
        
        await _eventRepository.AddEventAsync(newEvent);
    }
    public async Task UpdateEventAsync(int id, UpdateEventRequest request)
    {
        var eventToUpdate = await _eventRepository.GetEventByIdAsync(id);

        if (eventToUpdate == null)
            throw new NotFoundException(nameof(eventToUpdate), id);

        _mapper.Map(request, eventToUpdate);
        await _eventRepository.UpdateEventAsync(eventToUpdate);
    }

    public async Task DeleteEventAsync(int id)
    {
        await _eventRepository.DeleteEventAsync(id);
    }

    public async Task<IEnumerable<EventResponse>> GetEventsByCriteriaAsync(DateTime? date = null, string? location = null, string? category = null, int pageNumber = 1, int pageSize = 10)
    {
        var events = await _eventRepository.GetEventsByCriteriaAsync(date, location, category, pageNumber, pageSize);
        return _mapper.Map<IEnumerable<EventResponse>>(events);
    }

    public async Task UpdateEventsImageAsync(UpdateEventImageRequest request)
    {
        await _eventRepository.AddEventImageAsync(request.EventId, request.ImageBytes);
    }
}
