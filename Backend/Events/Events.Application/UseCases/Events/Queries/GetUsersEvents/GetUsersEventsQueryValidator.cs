using FluentValidation;
using Events.Application.Interfaces;

namespace Events.Application.UseCases.Events.Queries.GetUsersEvents;

public class GetUsersEventsQueryValidator : AbstractValidator<GetUsersEventsQuery>
{
    private readonly IGuidValidator _guidValidator;
    public GetUsersEventsQueryValidator(IGuidValidator guidValidator)
    {
        _guidValidator = guidValidator;

        RuleFor(x => x.UserId)
                .NotEmpty()
            .WithMessage("UserId is required.")
            .Must(userId => _guidValidator.IsValidGuid(userId))
            .WithMessage("UserId must be a valid GUID.");
    }

}
