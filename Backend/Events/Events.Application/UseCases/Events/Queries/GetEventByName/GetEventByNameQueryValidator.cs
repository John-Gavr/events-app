using FluentValidation;

namespace Events.Application.UseCases.Events.Queries.GetEventByName;

public class GetEventByNameQueryValidator : AbstractValidator<GetEventByNameQuery>
{

    public GetEventByNameQueryValidator()
    {
        RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Event name is required.")
                .MaximumLength(100)
                .WithMessage("Event name must not exceed 100 characters.");
    }
}
