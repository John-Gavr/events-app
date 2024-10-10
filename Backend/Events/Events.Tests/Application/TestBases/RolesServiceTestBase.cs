using Events.Application.DTOs.Roles.Requests;

namespace Events.Tests.Application.TestBases;

public class RolesServiceTestBase : ApplicationTestBase
{
    protected static readonly GetUserRolesRequest GetUserRolesRequest = new GetUserRolesRequest { UserId = UserId };
    protected static readonly SetUsersRolesRequest SetUsersRolesRequest = new SetUsersRolesRequest { UserId = UserId, RoleName = RoleName };
}
