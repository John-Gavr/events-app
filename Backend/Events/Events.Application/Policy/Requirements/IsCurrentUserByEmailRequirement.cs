using Microsoft.AspNetCore.Authorization;

namespace Events.Application.Policy.Requirements;

public class IsCurrentUserByEmailRequirement : IAuthorizationRequirement
{
    public IsCurrentUserByEmailRequirement() { }
}
