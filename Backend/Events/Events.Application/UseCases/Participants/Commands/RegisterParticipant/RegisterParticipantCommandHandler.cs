using AutoMapper;
using Events.Core.Entities.Exceptions;
using Events.Core.Entities;
using Events.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.Participants.Commands.RegisterParticipant;

public class RegisterParticipantCommandHandler : ParticipantsCommandHandlerBase, IRequestHandler<RegisterParticipantCommand, Unit>
{
    private readonly IEventRepository _eventRepository;
    public RegisterParticipantCommandHandler(IMapper mapper, IEventParticipantRepository eventParticipantRepository,
        UserManager<ApplicationUser> userManager, IEventRepository eventRepository) : base(mapper, eventParticipantRepository, userManager)
    {
        _eventRepository = eventRepository;
    }

    public async Task<Unit> Handle(RegisterParticipantCommand command, CancellationToken cancellationToken)
    {
        var participant = _mapper.Map<EventParticipant>(command);
        participant.UserId = Guid.Parse(command.UserId);

        var userEntity = await _userManager.FindByIdAsync(command.UserId);
        if (userEntity == null)
            throw new NotFoundException(nameof(userEntity), command.UserId);

        var eventEntity = await _eventRepository.GetEventByIdAsync(command.EventId, cancellationToken);
        if (eventEntity == null)
            throw new NotFoundException(nameof(eventEntity), command.EventId);

        if (eventEntity.Participants.Any(p => p.UserId.Equals(command.UserId)))
            throw new ParticipationAlredyExistException(eventEntity.Id, command.UserId);

        await _eventParticipantRepository.RegisterParticipantAsync(command.EventId, participant, cancellationToken);

        return Unit.Value;
    }
}
