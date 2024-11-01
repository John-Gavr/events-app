using FluentValidation;

namespace Events.Application.UseCases.Events.Commands.DeleteEvent;

public class DeleteEventCommandValidator : AbstractValidator<DeleteEventCommand>
{
    public DeleteEventCommandValidator()
    {
        RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Event Id must be greater than 0.");
    }
}
