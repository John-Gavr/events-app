using Events.Application.UseCases.Events.DTOs;
using MediatR;

namespace Events.Application.UseCases.Events.Queries.GetAllEvents;

public class GetAllEventsQuery : IRequest<EventsResponseDTO>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
