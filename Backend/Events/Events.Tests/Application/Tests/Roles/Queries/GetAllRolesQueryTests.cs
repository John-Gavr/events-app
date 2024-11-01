using Events.Application.UseCases.Events.Queries.GetAllEvents;
using Events.Application.UseCases.Roles.DTOs;
using Events.Application.UseCases.Roles.Queries.GetAllRoles;
using Events.Core.Entities;
using Moq;
using System.Threading;

namespace Events.Tests.Application.Tests.Roles.Queries;

public class GetAllRolesQueryTests : RolesTestBase
{
    private readonly GetAllRolesQueryHandler _handler;

    public GetAllRolesQueryTests()
    {
        _handler = new(_userManagerMock.Object, _roleManagerMock.Object, _mapperMock.Object);
    }

    [Fact]
    public void GetAllRoles_ReturnsRolesList()
    {
        _roleManagerMock.Setup(r => r.Roles).Returns(RolesList.AsQueryable());
        _mapperMock.Setup(m => m.Map<RoleNameResponseDTO>(It.IsAny<ApplicationRole>()))
            .Returns(new RoleNameResponseDTO { RoleName = RoleName });

        var result = _handler.Handle(getAllRolesQuery, _cancellationToken).Result;

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(RoleName, result.First().RoleName);
    }
}
