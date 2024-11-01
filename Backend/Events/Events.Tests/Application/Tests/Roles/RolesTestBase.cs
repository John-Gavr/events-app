using Events.Application.UseCases.Roles.Commands.SetUsersRoles;
using Events.Application.UseCases.Roles.Queries.GetAllRoles;
using Events.Application.UseCases.Roles.Queries.GetUserRoles;
using Events.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Events.Tests.Application.Tests.Roles;

public class RolesTestBase : ApplicationTestBase
{
    protected readonly Mock<RoleManager<ApplicationRole>> _roleManagerMock;
    public RolesTestBase()
    {
        var roleStoreMock = new Mock<IRoleStore<ApplicationRole>>();
        _roleManagerMock = new Mock<RoleManager<ApplicationRole>>(roleStoreMock.Object, null, null, null, null);
    }
    protected static readonly SetUsersRolesCommand setUsersRolesCommand = new SetUsersRolesCommand { UserId = userId, RoleName = RoleName };
    protected static readonly GetUserRolesQuery getUserRolesQuery = new GetUserRolesQuery { UserId = userId };
    protected static readonly GetAllRolesQuery getAllRolesQuery = new GetAllRolesQuery { };

}
