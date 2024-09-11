﻿using AutoMapper;
using Events.Application.DTOs.Participants.Requests.GetParticipantById;
using Events.Application.DTOs.Participants.Requests.GetParticipantsByEventId;
using Events.Application.DTOs.Participants.Requests.RegisterParticipant;
using Events.Application.DTOs.Participants.Requests.UnregisterParticipant;
using Events.Application.DTOs.Participants.Responses;
using Events.Application.Interfaces;
using Events.Core.Entities;
using Events.Core.Interfaces;

namespace Events.Application.Services;
public class EventParticipantService : IEventParticipantService
{
    private readonly IEventParticipantRepository _eventParticipantRepository;
    private readonly IMapper _mapper;

    public EventParticipantService(IEventParticipantRepository eventParticipantRepository, IMapper mapper)
    {
        _eventParticipantRepository = eventParticipantRepository;
        _mapper = mapper;
    }

    public async Task RegisterParticipantAsync(RegisterParticipantRequest request)
    {
        var participant = _mapper.Map<EventParticipant>(request);
        await _eventParticipantRepository.RegisterParticipantAsync(request.EventId, participant);
    }

    public async Task<IEnumerable<EventParticipantResponse>> GetParticipantsByEventIdAsync(GetParticipantsByEventIdRequest request)
    {
        var participants = await _eventParticipantRepository.GetParticipantsByEventIdAsync(request.EventId, request.PageNumber, request.PageSize);
        return _mapper.Map<IEnumerable<EventParticipantResponse>>(participants);
    }

    public async Task<EventParticipantResponse?> GetParticipantByIdAsync(GetParticipantByIdRequest request)
    {
        var participant = await _eventParticipantRepository.GetParticipantByIdAsync(request.Id);
        return _mapper.Map<EventParticipantResponse>(participant);
    }

    public async Task UnregisterParticipantAsync(UnregisterParticipantRequest request)
    {
        await _eventParticipantRepository.UnregisterParticipantAsync(request.EventId, request.ParticipantId);
    }
}