using Events.Application.Interfaces;

namespace Events.Application.Validation;

public class GuidValidator : IGuidValidator
{
    public bool IsValidGuid(string userId)
    {
        return Guid.TryParse(userId, out _);
    }
}
