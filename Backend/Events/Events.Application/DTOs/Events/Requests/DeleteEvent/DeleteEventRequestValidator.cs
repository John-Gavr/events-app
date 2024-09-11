using FluentValidation;

namespace Events.Application.DTOs.Events.Requests.DeleteEvent;

public class DeleteEventRequestValidator : AbstractValidator<DeleteEventRequest>
{
    public DeleteEventRequestValidator()
    {
        RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Event Id must be greater than 0.");
    }
}
