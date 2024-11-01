using FluentValidation;

namespace Events.Application.UseCases.Events.Commands.UpdateEventsImage;

public class UpdateEventImageCommandValidator : AbstractValidator<UpdateEventImageCommand>
{
    public UpdateEventImageCommandValidator()
    {
        RuleFor(x => x.EventId)
                .GreaterThan(0).WithMessage("EventId must be greater than zero.");

        RuleFor(x => x.ImageBytes)
            .NotEmpty().WithMessage("Image data is required.")
            .Must(bytes => bytes.Length > 0 && bytes.Length <= 2097152).WithMessage("Image size must not exceed 2MB.");
    }
}
