using Microsoft.AspNetCore.Identity;

namespace Events.Core.Entities;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole() : base()
    {

    }

    public ApplicationRole(string role) : base(role)
    {

    }

    public override string ToString()
    {
        return base.ToString();
    }
}
