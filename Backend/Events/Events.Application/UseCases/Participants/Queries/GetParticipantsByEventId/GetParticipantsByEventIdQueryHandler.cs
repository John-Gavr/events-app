using AutoMapper;
using Events.Application.UseCases.Participants.DTOs;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Events.Core.Interfaces;
using Events.Infrastructure.Data.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.Participants.Queries.GetParticipantsByEventId;

public class GetParticipantsByEventIdQueryHandler : ParticipantsQueryHandlerBase, IRequestHandler<GetParticipantsByEventIdQuery, IEnumerable<EventParticipantResponseDTO>>
{
    private readonly IEventRepository _eventRepository;
    public GetParticipantsByEventIdQueryHandler(IMapper mapper, IEventParticipantRepository eventParticipantRepository,
        UserManager<ApplicationUser> userManager, IEventRepository eventRepository) : base(mapper, eventParticipantRepository, userManager)
    {
        _eventRepository = eventRepository;
    }

    public async Task<IEnumerable<EventParticipantResponseDTO>> Handle(GetParticipantsByEventIdQuery query, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetEventByIdAsync(query.EventId, cancellationToken);
        if (eventEntity == null)
            throw new NotFoundException(nameof(eventEntity), query.EventId);

        var participants = await _eventParticipantRepository.GetParticipantsByEventIdAsync(query.EventId, query.PageNumber, query.PageSize, cancellationToken);
        return _mapper.Map<IEnumerable<EventParticipantResponseDTO>>(participants);

    }
}
