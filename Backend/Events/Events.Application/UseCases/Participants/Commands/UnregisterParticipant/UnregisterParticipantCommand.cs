using MediatR;

namespace Events.Application.UseCases.Participants.Commands.UnregisterParticipant;

public class UnregisterParticipantCommand : IRequest<Unit>
{
    public string UserId { get; set; } = String.Empty;
    public int EventId { get; set; }
}
