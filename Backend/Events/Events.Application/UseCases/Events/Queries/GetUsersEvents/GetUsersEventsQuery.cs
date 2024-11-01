using Events.Application.UseCases.Events.DTOs;
using MediatR;

namespace Events.Application.UseCases.Events.Queries.GetUsersEvents;

public class GetUsersEventsQuery : IRequest<EventsResponseDTO>
{
    public string UserId { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
