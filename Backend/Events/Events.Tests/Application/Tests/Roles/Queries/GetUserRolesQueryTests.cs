using Events.Core.Entities.Exceptions;
using Events.Core.Entities;
using System.Threading;
using Events.Application.UseCases.Roles.Queries.GetUserRoles;
using Moq;

namespace Events.Tests.Application.Tests.Roles.Queries;

public class GetUserRolesQueryTests : RolesTestBase
{
    private readonly GetUserRolesQueryHandler _handler;

    public GetUserRolesQueryTests()
    {
        _handler = new(_userManagerMock.Object, _roleManagerMock.Object, _mapperMock.Object);
    }
    [Fact]
    public async Task GetUsersRoleAsync_UserExists_ReturnsUserRoles()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(testUser);
        _userManagerMock.Setup(u => u.GetRolesAsync(testUser)).ReturnsAsync(userRoles);

        var result = await _handler.Handle(getUserRolesQuery, _cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal(RoleName, result.First().RoleName);
    }

    [Fact]
    public async Task GetUsersRoleAsync_UserNotFound_ThrowsNotFoundException()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync((ApplicationUser?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(getUserRolesQuery, _cancellationToken));
    }
}
