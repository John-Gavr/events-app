using Events.Application.Policy.Requirements;
using Events.Core.Entities.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Events.Web.Host.Policy.Handlers;

public class IsCurrentUserRequirementHandler : AuthorizationHandler<IsCurrentUserRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IsCurrentUserRequirementHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsCurrentUserRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new UnauthorizedOperationException("User ID is invalid.");
        }

        var userIdString = httpContext.Request.Query["UserId"].ToString();

        if (Guid.TryParse(userIdString, out var userId))
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var currentUserId))
            {
                throw new UnauthorizedOperationException("User ID is invalid.");
            }

            if (userId == currentUserId)
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
            throw new UnauthorizedOperationException("User ID is invalid.");
        }
    }
}

