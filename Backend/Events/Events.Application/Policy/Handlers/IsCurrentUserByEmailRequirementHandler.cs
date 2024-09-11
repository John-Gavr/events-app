using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Events.Application.Policy.Requirements;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Events.Application.Policy.Handlers;

public class IsCurrentUserByEmailRequirementHandler : AuthorizationHandler<IsCurrentUserRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    public IsCurrentUserByEmailRequirementHandler(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsCurrentUserRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new UnauthorizedOperationException("User ID is invalid.");
        }

        var userEmailString = httpContext.Request.Query["Email"].ToString();
        var user = await _userManager.FindByEmailAsync(userEmailString);
        if (user == null) 
            throw new NotFoundException(nameof(ApplicationUser), userEmailString);
        
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var currentUserId))
        {
            throw new UnauthorizedOperationException("User ID is invalid.");
        }

        if (user.Id == currentUserId.ToString())
        {
            context.Succeed(requirement);
        }
        else
        {
            throw new UnauthorizedOperationException("User does not have permission.");
        }
    }
}

