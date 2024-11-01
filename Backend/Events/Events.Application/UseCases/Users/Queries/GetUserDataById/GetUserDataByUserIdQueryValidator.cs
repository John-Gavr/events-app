using Events.Application.Interfaces;
using FluentValidation;

namespace Events.Application.UseCases.Users.Queries.GetUserDataById;

public class GetUserDataByUserIdQueryValidator : AbstractValidator<GetUserDataByUserIdQuery>
{
    private readonly IGuidValidator _guidValidator;
    public GetUserDataByUserIdQueryValidator(IGuidValidator guidValidator)
    {
        _guidValidator = guidValidator;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.")
            .Must(userId => _guidValidator.IsValidGuid(userId))
            .WithMessage("UserId must be a valid GUID.");
    }
}
