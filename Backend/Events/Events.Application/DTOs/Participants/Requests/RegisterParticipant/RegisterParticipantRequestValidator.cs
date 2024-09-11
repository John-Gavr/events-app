using Events.Application.Interfaces;
using FluentValidation;

namespace Events.Application.DTOs.Participants.Requests.RegisterParticipant;

public class RegisterParticipantRequestValidator : AbstractValidator<RegisterParticipantRequest>
{
    private readonly IGuidValidator _guidValidator;
    public RegisterParticipantRequestValidator(IGuidValidator guidValidator)
    {
        _guidValidator = guidValidator;
        RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.");

        RuleFor(x => x.RegistrationDate)
            .NotEmpty().WithMessage("Registration date is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Registration date cannot be in the future.");

        RuleFor(x => x.UserId).Must(userId => _guidValidator.IsValidGuid(userId))
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.EventId)
            .GreaterThan(0).WithMessage("EventId must be greater than zero.");

    }

}
