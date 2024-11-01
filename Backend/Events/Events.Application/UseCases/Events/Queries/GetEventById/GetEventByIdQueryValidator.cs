using FluentValidation;

namespace Events.Application.UseCases.Events.Queries.GetEventById;

public class GetEventByIdQueryValidator : AbstractValidator<GetEventByIdQuery>
{
    public GetEventByIdQueryValidator()
    {
        RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Event Id must be greater than 0.");
    }
}
