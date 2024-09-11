using Events.Application.Interfaces;
using FluentValidation;

namespace Events.Application.DTOs.Roles.Requests.GetUserRoles;

public class GetUserRolesRequestValidator : AbstractValidator<GetUserRolesRequest>
{

    private readonly IGuidValidator _guidValidator;
    public GetUserRolesRequestValidator(IGuidValidator guidValidator)
    {
        _guidValidator = guidValidator;

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .Must(userId => _guidValidator.IsValidGuid(userId)).WithMessage("UserId must be a valid GUID.");
    }
}
