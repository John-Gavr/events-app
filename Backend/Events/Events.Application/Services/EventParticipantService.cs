using AutoMapper;
using Events.Application.DTOs.Participants.Requests.GetParticipantById;
using Events.Application.DTOs.Participants.Requests.GetParticipantsByEventId;
using Events.Application.DTOs.Participants.Requests.RegisterParticipant;
using Events.Application.DTOs.Participants.Requests.UnregisterParticipant;
using Events.Application.DTOs.Participants.Responses;
using Events.Application.Interfaces;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Events.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.Services;
public class EventParticipantService : IEventParticipantService
{
    private readonly IEventParticipantRepository _eventParticipantRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    public EventParticipantService(IEventParticipantRepository eventParticipantRepository, 
        IEventRepository eventRepository, 
        IMapper mapper, 
        UserManager<ApplicationUser> userManager)
    {
        _eventParticipantRepository = eventParticipantRepository;
        _eventRepository = eventRepository;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task RegisterParticipantAsync(RegisterParticipantRequest request, string userId)
    {
        var participant = _mapper.Map<EventParticipant>(request);
        participant.UserId = Guid.Parse(userId);

        var userEntity = await _userManager.FindByIdAsync(userId);
        if (userEntity == null)
            throw new NotFoundException(nameof(userEntity), userId);

        var eventEntity = await _eventRepository.GetEventByIdAsync(request.EventId);
        if(eventEntity == null) 
            throw new NotFoundException(nameof(eventEntity), request.EventId);

        if (eventEntity.Participants.Any(p => p.UserId.Equals(userId)))
            throw new ParticipationAlredyExistException(eventEntity.Id, userId);

        await _eventParticipantRepository.RegisterParticipantAsync(request.EventId, participant);
    }

    public async Task<IEnumerable<EventParticipantResponse>> GetParticipantsByEventIdAsync(GetParticipantsByEventIdRequest request)
    {
        var eventEntity = await _eventRepository.GetEventByIdAsync(request.EventId);
        if (eventEntity == null)
            throw new NotFoundException(nameof(eventEntity), request.EventId);

        var participants = await _eventParticipantRepository.GetParticipantsByEventIdAsync(request.EventId, request.PageNumber, request.PageSize);
        return _mapper.Map<IEnumerable<EventParticipantResponse>>(participants);
    }

    public async Task<EventParticipantResponse?> GetParticipantByUserIdAsync(GetParticipantByUserIdRequest request)
    {
        var userEntity = await _userManager.FindByIdAsync(request.UserId);
        if (userEntity == null)
            throw new NotFoundException(nameof(userEntity), request.UserId);

        var participant = await _eventParticipantRepository.GetParticipantByUserIdAsync(request.UserId);
        return _mapper.Map<EventParticipantResponse>(participant);
    }

    public async Task UnregisterParticipantAsync(UnregisterParticipantRequest request, string userId)
    {
        var userEntity = await _userManager.FindByIdAsync(userId);
        if (userEntity == null)
            throw new NotFoundException(nameof(userEntity), userId);

        var participantEntity = await _eventParticipantRepository.GetParticipantByUserIdAsync(userId);
        if (participantEntity == null)
            throw new NotFoundException(nameof(participantEntity), userId);

        await _eventParticipantRepository.UnregisterParticipantAsync(request.EventId, userId);
    }
}
