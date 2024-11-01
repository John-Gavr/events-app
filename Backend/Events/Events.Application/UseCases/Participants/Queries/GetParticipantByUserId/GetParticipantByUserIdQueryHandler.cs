using AutoMapper;
using Events.Application.UseCases.Participants.DTOs;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Events.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.Participants.Queries.GetParticipantByUserId;

public class GetParticipantByUserIdQueryHandler : ParticipantsQueryHandlerBase, IRequestHandler<GetParticipantByUserIdQuery, EventParticipantResponseDTO>
{
    public GetParticipantByUserIdQueryHandler(IMapper mapper, IEventParticipantRepository eventParticipantRepository, UserManager<ApplicationUser> userManager) : base(mapper, eventParticipantRepository, userManager)
    { }

    public async Task<EventParticipantResponseDTO> Handle(GetParticipantByUserIdQuery query, CancellationToken cancellationToken)
    {
        var userEntity = await _userManager.FindByIdAsync(query.UserId);
        if (userEntity == null)
            throw new NotFoundException(nameof(userEntity), query.UserId);

        var participant = await _eventParticipantRepository.GetParticipantByUserIdAsync(query.UserId, cancellationToken);
        return _mapper.Map<EventParticipantResponseDTO>(participant);
    }
}
