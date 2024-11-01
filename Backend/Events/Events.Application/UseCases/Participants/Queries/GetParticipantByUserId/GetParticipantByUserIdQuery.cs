using Events.Application.UseCases.Participants.DTOs;
using MediatR;

namespace Events.Application.UseCases.Participants.Queries.GetParticipantByUserId;

public class GetParticipantByUserIdQuery : IRequest<EventParticipantResponseDTO>
{
    public string UserId { get; set; } = string.Empty;
}
