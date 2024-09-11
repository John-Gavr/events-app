using FluentValidation;

namespace Events.Application.DTOs.Participants.Requests.GetParticipantById;

public class GetParticipantByIdRequestValidator : AbstractValidator<GetParticipantByIdRequest>
{
    public GetParticipantByIdRequestValidator()
    {
        RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Participant Id must be greater than 0.");
    }
}
