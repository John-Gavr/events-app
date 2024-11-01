using Events.Application.Interfaces;
using FluentValidation;

namespace Events.Application.UseCases.Roles.Commands.SetUsersRoles;

public class SetUsersRolesCommandValidator : AbstractValidator<SetUsersRolesCommand>
{

    private readonly IGuidValidator _guidValidator;
    public SetUsersRolesCommandValidator(IGuidValidator guidValidator)
    {
        _guidValidator = guidValidator;
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .Must(userId => _guidValidator.IsValidGuid(userId)).WithMessage("UserId must be a valid GUID.");

        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("RoleName is required.");
    }
}
