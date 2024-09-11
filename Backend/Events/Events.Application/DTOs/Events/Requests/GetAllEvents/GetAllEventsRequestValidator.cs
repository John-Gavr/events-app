using FluentValidation;

namespace Events.Application.DTOs.Events.Requests.GetAllEvents;

public class GetAllEventsRequestValidator : AbstractValidator<GetAllEventsRequest>
{
    public GetAllEventsRequestValidator()
    {
        RuleFor(x => x.PageNumber)
               .GreaterThanOrEqualTo(1).WithMessage("Page number must be 1 or greater.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1.")
            .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100.");
    }
}
