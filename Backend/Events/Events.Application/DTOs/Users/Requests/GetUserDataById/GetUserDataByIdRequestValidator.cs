using Events.Application.Interfaces;
using FluentValidation;

namespace Events.Application.DTOs.Users.Requests.GetUserDataById;

public class GetUserDataByIdRequestValidator : AbstractValidator<GetUserDataByIdRequest>
{
    private readonly IGuidValidator _guidValidator;
    public GetUserDataByIdRequestValidator(IGuidValidator guidValidator)
    {
        _guidValidator = guidValidator;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.")
            .Must(userId => _guidValidator.IsValidGuid(userId))
            .WithMessage("UserId must be a valid GUID.");
    }
}
