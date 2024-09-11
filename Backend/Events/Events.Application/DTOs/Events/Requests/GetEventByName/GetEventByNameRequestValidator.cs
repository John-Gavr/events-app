using FluentValidation;

namespace Events.Application.DTOs.Events.Requests.GetEventByName;

public class GetEventByNameRequestValidator : AbstractValidator<GetEventByNameRequest>
{

    public GetEventByNameRequestValidator()
    {
        RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Event name is required.")
                .MaximumLength(100)
                .WithMessage("Event name must not exceed 100 characters.");
    }
}
