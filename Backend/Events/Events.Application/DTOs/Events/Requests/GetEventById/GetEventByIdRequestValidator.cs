using FluentValidation;

namespace Events.Application.DTOs.Events.Requests.GetEventById;

public class GetEventByIdRequestValidator : AbstractValidator<GetEventByIdRequest>
{
    public GetEventByIdRequestValidator()
    {
        RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Event Id must be greater than 0.");
    }
}
