using FluentValidation;

namespace Events.Application.DTOs.Participants.Requests.UnregisterParticipant;

public class UnregisterParticipantRequestValidator : AbstractValidator<UnregisterParticipantRequest>
{
    public UnregisterParticipantRequestValidator()
    {
        RuleFor(x => x.EventId)
                .GreaterThan(0).WithMessage("Event Id must be greater than zero.");

        RuleFor(x => x.ParticipantId)
            .GreaterThan(0).WithMessage("Participant Id must be greater than zero.");
    }
}
