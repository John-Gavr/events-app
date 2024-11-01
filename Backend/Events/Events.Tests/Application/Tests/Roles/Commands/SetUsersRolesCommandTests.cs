using Events.Core.Entities.Exceptions;
using Events.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading;
using Events.Application.UseCases.Roles.Commands.SetUsersRoles;

namespace Events.Tests.Application.Tests.Roles.Commands;

public class SetUsersRolesCommandTests : RolesTestBase
{
    private readonly SetUsersRolesCommandHandler _handler;

    public SetUsersRolesCommandTests()
    {
        _handler = new(_userManagerMock.Object, _roleManagerMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task SetUsersRoleAsync_UserAndRoleExist_AddsRoleToUser()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(testUser);
        _roleManagerMock.Setup(r => r.FindByNameAsync(RoleName)).ReturnsAsync(TestRole);
        _userManagerMock.Setup(u => u.AddToRoleAsync(testUser, RoleName)).ReturnsAsync(IdentityResult.Success);

        await _handler.Handle(setUsersRolesCommand, _cancellationToken);

        _userManagerMock.Verify(u => u.AddToRoleAsync(testUser, RoleName), Times.Once);
    }

    [Fact]
    public async Task SetUsersRoleAsync_UserNotFound_ThrowsNotFoundException()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync((ApplicationUser?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(setUsersRolesCommand, _cancellationToken));
    }

    [Fact]
    public async Task SetUsersRoleAsync_RoleNotFound_ThrowsNotFoundException()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(testUser);
        _roleManagerMock.Setup(r => r.FindByNameAsync(RoleName)).ReturnsAsync((ApplicationRole?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(setUsersRolesCommand, _cancellationToken));
    }
}
