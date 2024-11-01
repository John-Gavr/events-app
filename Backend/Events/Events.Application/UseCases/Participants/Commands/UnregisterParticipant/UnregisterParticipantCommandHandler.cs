using AutoMapper;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Events.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.Participants.Commands.UnregisterParticipant;

public class UnregisterParticipantCommandHandler : ParticipantsCommandHandlerBase, IRequestHandler<UnregisterParticipantCommand, Unit>
{
    public UnregisterParticipantCommandHandler(IMapper mapper, IEventParticipantRepository eventParticipantRepository,
        UserManager<ApplicationUser> userManager) : base(mapper, eventParticipantRepository, userManager)
    { }

    public async Task<Unit> Handle(UnregisterParticipantCommand command, CancellationToken cancellationToken)
    {
        var userEntity = await _userManager.FindByIdAsync(command.UserId);
        if (userEntity == null)
            throw new NotFoundException(nameof(userEntity), command.UserId);

        var participantEntity = await _eventParticipantRepository.GetParticipantByUserIdAsync(command.UserId, cancellationToken);
        if (participantEntity == null)
            throw new NotFoundException(nameof(participantEntity), command.UserId);

        await _eventParticipantRepository.UnregisterParticipantAsync(command.EventId, command.UserId, cancellationToken);

        return Unit.Value;
    }
}
