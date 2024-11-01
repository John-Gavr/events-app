using Events.Application.Interfaces;
using FluentValidation;

namespace Events.Application.UseCases.Roles.Queries.GetUserRoles;

public class GetUserRolesQueryValidator : AbstractValidator<GetUserRolesQuery>
{

    private readonly IGuidValidator _guidValidator;
    public GetUserRolesQueryValidator(IGuidValidator guidValidator)
    {
        _guidValidator = guidValidator;

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .Must(userId => _guidValidator.IsValidGuid(userId)).WithMessage("UserId must be a valid GUID.");
    }
}
