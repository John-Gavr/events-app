using AutoMapper;
using Events.Application.DTOs.Events.Requests.CreateEvent;
using Events.Application.DTOs.Events.Requests.UpdateEvent;
using Events.Application.DTOs.Events.Requests.UpdateEventsImage;
using Events.Application.DTOs.Events.Responces;
using Events.Application.Interfaces;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Events.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public EventService(IEventRepository eventRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<EventsResponse> GetAllEventsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var events = await _eventRepository.GetAllEventsAsync(pageNumber, pageSize, cancellationToken);
        var totalCount = await _eventRepository.GetNumberOfAllEventsAsync(cancellationToken);
        return new EventsResponse()
        {
            Events = _mapper.Map<IEnumerable<EventResponse>>(events),
            TotalCount = totalCount
        };
    }

    public async Task<EventResponse?> GetEventByIdAsync(int id, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetEventByIdAsync(id, cancellationToken);
        if (eventEntity == null)
            throw new NotFoundException(nameof(eventEntity), id);

        return _mapper.Map<EventResponse>(eventEntity);
    }

    public async Task<EventResponse?> GetEventByNameAsync(string name, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetEventByNameAsync(name, cancellationToken);
        if (eventEntity == null)
            throw new NotFoundException(nameof(eventEntity), name);

        return _mapper.Map<EventResponse>(eventEntity);
    }

    public async Task AddEventAsync(CreateEventRequest request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetEventByNameAsync(request.Name, cancellationToken);
        if (eventEntity != null)
            throw new EventAlredyExistException(request.Name);

        var newEvent = _mapper.Map<Event>(request);
        await _eventRepository.AddEventAsync(newEvent, cancellationToken);
    }

    public async Task UpdateEventAsync(int id, UpdateEventRequest request, CancellationToken cancellationToken)
    {
        var eventToUpdate = await _eventRepository.GetEventByIdAsync(id, cancellationToken);

        if (eventToUpdate == null)
            throw new NotFoundException(nameof(eventToUpdate), id);

        _mapper.Map(request, eventToUpdate);
        await _eventRepository.UpdateEventAsync(eventToUpdate, cancellationToken);
    }

    public async Task DeleteEventAsync(int id, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetEventByIdAsync(id, cancellationToken);
        if (eventEntity == null)
            throw new NotFoundException(nameof(eventEntity), id);

        await _eventRepository.DeleteEventAsync(id, cancellationToken);
    }

    public async Task<EventsResponse> GetEventsByCriteriaAsync(CancellationToken cancellationToken, DateTime? date = null, string? location = null, string? category = null, int pageNumber = 1, int pageSize = 10)
    {
        var events = await _eventRepository.GetEventsByCriteriaAsync(cancellationToken, date, location, category, pageNumber, pageSize);
        int totalCount = await _eventRepository.GetNumberOfAllEventsByCriteriaAsync(cancellationToken, date, location, category);

        return new EventsResponse
        {
            Events = _mapper.Map<IEnumerable<EventResponse>>(events),
            TotalCount = totalCount
        };
    }

    public async Task UpdateEventsImageAsync(UpdateEventImageRequest request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetEventByIdAsync(request.EventId, cancellationToken);
        if (eventEntity == null)
            throw new NotFoundException(nameof(eventEntity), request.EventId);

        await _eventRepository.AddEventImageAsync(request.EventId, request.ImageBytes, cancellationToken);
    }

    public async Task<EventsResponse> GetEventsByUserIdAsync(string userId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var userEntity = await _userManager.FindByIdAsync(userId);
        if (userEntity == null)
            throw new NotFoundException(nameof(userEntity), userId);

        var events = await _eventRepository.GetEventsByUserIdAsync(userId, pageNumber, pageSize, cancellationToken);
        var totalCount = await _eventRepository.GetUserEventsCountAsync(userId, cancellationToken);
        return new EventsResponse
        {
            Events = _mapper.Map<IEnumerable<EventResponse>>(events),
            TotalCount = totalCount
        };
    }
}
