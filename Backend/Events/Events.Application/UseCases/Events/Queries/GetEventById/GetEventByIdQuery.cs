using Events.Application.UseCases.Events.DTOs;
using MediatR;

namespace Events.Application.UseCases.Events.Queries.GetEventById;

public class GetEventByIdQuery : IRequest<EventResponseDTO>
{
    public int Id { get; set; }
}
