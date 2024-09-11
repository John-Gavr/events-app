using Microsoft.AspNetCore.Authorization;

namespace Events.Application.Policy.Requirements;

public class IsParticipantRequirement : IAuthorizationRequirement
{
    public IsParticipantRequirement() { }
}
