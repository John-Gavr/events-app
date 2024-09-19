using FluentValidation;
using Events.Application.Interfaces;

namespace Events.Application.DTOs.Events.Requests.GetUsersEvents;

public class GetUsersEventsRequestValidator : AbstractValidator<GetUsersEventsRequest>
{
    private readonly IGuidValidator _guidValidator;
    public GetUsersEventsRequestValidator(IGuidValidator guidValidator)
    {
        _guidValidator = guidValidator;

        RuleFor(x => x.UserId)
                .NotEmpty()
            .WithMessage("UserId is required.")
            .Must(userId => _guidValidator.IsValidGuid(userId))
            .WithMessage("UserId must be a valid GUID.");
    }

}
