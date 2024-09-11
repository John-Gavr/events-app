using Events.Application.Interfaces;
using FluentValidation;

namespace Events.Application.DTOs.Participants.Requests.GetParticipantById;

public class GetParticipantByUserIdRequestValidator : AbstractValidator<GetParticipantByUserIdRequest>
{
    private readonly IGuidValidator _guidValidator;
    public GetParticipantByUserIdRequestValidator(IGuidValidator guidValidator)
    {
        _guidValidator = guidValidator;

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .Must(userId => _guidValidator.IsValidGuid(userId)).WithMessage("UserId must be a valid GUID.");
    }
}
