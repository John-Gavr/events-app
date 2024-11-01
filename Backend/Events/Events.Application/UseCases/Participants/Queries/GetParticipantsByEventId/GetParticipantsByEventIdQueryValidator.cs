using FluentValidation;

namespace Events.Application.UseCases.Participants.Queries.GetParticipantsByEventId;

public class GetParticipantsByEventIdQueryValidator : AbstractValidator<GetParticipantsByEventIdQuery>
{

    public GetParticipantsByEventIdQueryValidator()
    {
        RuleFor(x => x.EventId)
                .GreaterThan(0).WithMessage("Event Id must be greater than zero.");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than zero.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than zero.");
    }

}
