using Events.Application.Policy.Requirements;
using Events.Core.Entities.Exceptions;
using Events.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Events.Web.Host.Policy.Handlers;

public class IsParticipantRequirementHandler : AuthorizationHandler<IsCurrentUserRequirement>
{
    private readonly IEventRepository _eventRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IsParticipantRequirementHandler(IEventRepository eventRepository, IHttpContextAccessor httpContextAccessor)
    {
        _eventRepository = eventRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsCurrentUserRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new UnauthorizedOperationException("User ID is invalid.");
        }

        // Извлечение данных из запроса
        var eventIdString = httpContext.Request.Query["EventId"].ToString();
        var participantIdString = httpContext.Request.Query["ParticipantId"].ToString();

        if (int.TryParse(eventIdString, out var eventId) &&
            int.TryParse(participantIdString, out var participantId))
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedOperationException("User ID is invalid.");
            }

            var eventEntity = await _eventRepository.GetEventByIdAsync(eventId);
            var participant = eventEntity?.Participants.FirstOrDefault(p => p.Id == participantId);

            if (participant != null && participant.UserId == userId)
            {
                context.Succeed(requirement);
            }
            else
            {
                throw new UnauthorizedOperationException("User does not have permission.");
            }
        }
        else
        {
            throw new UnauthorizedOperationException("Event ID or Participant ID is invalid.");
        }
    }
}

