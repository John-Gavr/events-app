using Events.Application.Interfaces;
using FluentValidation;

namespace Events.Application.DTOs.Users.Requests.GetUserDataById;

public class GetUserDataByUserIdRequestValidator : AbstractValidator<GetUserDataByUserIdRequest>
{
    private readonly IGuidValidator _guidValidator;
    public GetUserDataByUserIdRequestValidator(IGuidValidator guidValidator)
    {
        _guidValidator = guidValidator;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.")
            .Must(userId => _guidValidator.IsValidGuid(userId))
            .WithMessage("UserId must be a valid GUID.");
    }
}
