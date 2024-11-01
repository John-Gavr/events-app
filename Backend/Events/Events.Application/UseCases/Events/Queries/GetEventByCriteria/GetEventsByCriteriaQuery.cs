using Events.Application.UseCases.Events.DTOs;
using MediatR;

namespace Events.Application.UseCases.Events.Queries.GetEventByCriteria;

public class GetEventsByCriteriaQuery : IRequest<EventsResponseDTO>
{
    public DateTime? Date { get; set; }
    public string? Location { get; set; }
    public string? Category { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
