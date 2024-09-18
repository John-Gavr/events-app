using FluentValidation;

namespace Events.Application.DTOs.Events.Requests.UpdateEvent;

public class UpdateEventRequestValidator : AbstractValidator<UpdateEventRequest>
{

    public UpdateEventRequestValidator()
    {
        RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Event name is required.")
                .MaximumLength(100).WithMessage("Event name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.EventDateTime)
            .NotEmpty().WithMessage("Event date is required.")
            .Must(date => date > DateTime.Now).WithMessage("Event date must be in the future.");

        RuleFor(x => x.Location)
            .MaximumLength(200).WithMessage("Location must not exceed 200 characters.");

        RuleFor(x => x.Category)
            .MaximumLength(50).WithMessage("Category must not exceed 50 characters.");

        RuleFor(x => x.MaxParticipants)
            .GreaterThan(0).WithMessage("Max participants must be greater than 0.");
    }
}
