using FluentValidation;

namespace Events.Application.UseCases.Events.Queries.GetEventByCriteria;

public class GetEventsByCriteriaQueryValidator : AbstractValidator<GetEventsByCriteriaQuery>
{

    public GetEventsByCriteriaQueryValidator()
    {
        RuleFor(x => x.Date)
                .GreaterThanOrEqualTo(DateTime.Today)
                .When(x => x.Date.HasValue)
                .WithMessage("Event date cannot be in the past.");

        RuleFor(x => x.Location)
            .MaximumLength(200)
            .WithMessage("Location must not exceed 200 characters.");

        RuleFor(x => x.Category)
            .MaximumLength(50)
            .WithMessage("Category must not exceed 50 characters.");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be 1 or greater.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be at least 1.")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size cannot exceed 100.");
    }

}
