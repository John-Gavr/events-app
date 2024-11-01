using FluentValidation;

namespace Events.Application.UseCases.Participants.Commands.UnregisterParticipant;

public class UnregisterParticipantCommandValidator : AbstractValidator<UnregisterParticipantCommand>
{
    public UnregisterParticipantCommandValidator()
    {
        RuleFor(x => x.EventId)
                .GreaterThan(0).WithMessage("Event Id must be greater than zero.");
    }
}
