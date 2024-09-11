using Microsoft.AspNetCore.Authorization;

namespace Events.Application.Policy.Requirements;

public class IsCurrentUserRequirement : IAuthorizationRequirement
{
    public IsCurrentUserRequirement() { }
}
