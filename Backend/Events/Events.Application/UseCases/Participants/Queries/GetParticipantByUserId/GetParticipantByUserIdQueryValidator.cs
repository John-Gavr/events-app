using Events.Application.Interfaces;
using FluentValidation;

namespace Events.Application.UseCases.Participants.Queries.GetParticipantByUserId;

public class GetParticipantByUserIdQueryValidator : AbstractValidator<GetParticipantByUserIdQuery>
{
    private readonly IGuidValidator _guidValidator;
    public GetParticipantByUserIdQueryValidator(IGuidValidator guidValidator)
    {
        _guidValidator = guidValidator;

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .Must(userId => _guidValidator.IsValidGuid(userId)).WithMessage("UserId must be a valid GUID.");
    }
}
