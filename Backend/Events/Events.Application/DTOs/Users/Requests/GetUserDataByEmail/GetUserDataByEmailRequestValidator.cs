using FluentValidation;

namespace Events.Application.DTOs.Users.Requests.GetUserDataByEmail;

public class GetUserDataByEmailRequestValidator : AbstractValidator<GetUserDataByEmailRequest>
{
    public GetUserDataByEmailRequestValidator()
    {
        RuleFor(x => x.Email)
           .NotEmpty()
           .WithMessage("Email is required.")
           .EmailAddress()
           .WithMessage("Invalid email address format.");
    }
}
