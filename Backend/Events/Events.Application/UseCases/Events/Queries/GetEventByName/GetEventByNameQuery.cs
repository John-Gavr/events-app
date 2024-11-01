using Events.Application.UseCases.Events.DTOs;
using MediatR;

namespace Events.Application.UseCases.Events.Queries.GetEventByName;

public class GetEventByNameQuery : IRequest<EventResponseDTO>
{
    public string Name { get; set; } = string.Empty;
}
