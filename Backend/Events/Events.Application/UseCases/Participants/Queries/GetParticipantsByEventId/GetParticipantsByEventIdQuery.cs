using Events.Application.UseCases.Participants.DTOs;
using MediatR;

namespace Events.Application.UseCases.Participants.Queries.GetParticipantsByEventId;

public class GetParticipantsByEventIdQuery : IRequest<IEnumerable<EventParticipantResponseDTO>>
{
    public int EventId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
