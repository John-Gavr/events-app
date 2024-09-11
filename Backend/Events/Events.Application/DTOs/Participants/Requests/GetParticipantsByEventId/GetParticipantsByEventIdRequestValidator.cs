using FluentValidation;

namespace Events.Application.DTOs.Participants.Requests.GetParticipantsByEventId;

public class GetParticipantsByEventIdRequestValidator : AbstractValidator<GetParticipantsByEventIdRequest>
{

    public GetParticipantsByEventIdRequestValidator()
    {
        RuleFor(x => x.EventId)
                .GreaterThan(0).WithMessage("Event Id must be greater than zero.");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than zero.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than zero.");
    }

}
