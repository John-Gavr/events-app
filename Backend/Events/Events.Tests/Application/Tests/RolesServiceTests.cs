using Events.Application.DTOs.Roles.Responses;
using Events.Application.Services;
using Events.Core.Entities.Exceptions;
using Events.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using Events.Tests.Application.TestBases;

namespace Events.Tests.Application.Services.Tests;
public class RolesServiceTests : RolesServiceTestBase
{
    private readonly RolesService _rolesService;
    private readonly Mock<RoleManager<ApplicationRole>> _roleManagerMock;
    public RolesServiceTests()
    {
        var roleStoreMock = new Mock<IRoleStore<ApplicationRole>>();
        _roleManagerMock = new Mock<RoleManager<ApplicationRole>>(roleStoreMock.Object, null, null, null, null);

        _rolesService = new RolesService(_roleManagerMock.Object, _userManagerMock.Object, _mapperMock.Object);
    }

    [Fact]
    public void GetAllRoles_ReturnsRolesList()
    {
        _roleManagerMock.Setup(r => r.Roles).Returns(RolesList.AsQueryable());
        _mapperMock.Setup(m => m.Map<RoleResponse>(It.IsAny<ApplicationRole>()))
            .Returns(new RoleResponse { RoleName = RoleName });

        var result = _rolesService.GetAllRoles(_cancellationToken);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(RoleName, result.First().RoleName);
    }

    [Fact]
    public async Task GetUsersRoleAsync_UserExists_ReturnsUserRoles()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(UserId)).ReturnsAsync(TestUser);
        _userManagerMock.Setup(u => u.GetRolesAsync(TestUser)).ReturnsAsync(UserRoles);

        var result = await _rolesService.GetUsersRoleAsync(GetUserRolesRequest, _cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(RoleName, result.First().RoleName);
    }

    [Fact]
    public async Task GetUsersRoleAsync_UserNotFound_ThrowsNotFoundException()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(UserId)).ReturnsAsync((ApplicationUser?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _rolesService.GetUsersRoleAsync(GetUserRolesRequest, _cancellationToken));
    }

    [Fact]
    public async Task SetUsersRoleAsync_UserAndRoleExist_AddsRoleToUser()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(UserId)).ReturnsAsync(TestUser);
        _roleManagerMock.Setup(r => r.FindByNameAsync(RoleName)).ReturnsAsync(TestRole);
        _userManagerMock.Setup(u => u.AddToRoleAsync(TestUser, RoleName)).ReturnsAsync(IdentityResult.Success);

        await _rolesService.SetUsersRoleAsync(SetUsersRolesRequest, _cancellationToken);

        _userManagerMock.Verify(u => u.AddToRoleAsync(TestUser, RoleName), Times.Once);
    }

    [Fact]
    public async Task SetUsersRoleAsync_UserNotFound_ThrowsNotFoundException()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(UserId)).ReturnsAsync((ApplicationUser?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _rolesService.SetUsersRoleAsync(SetUsersRolesRequest, _cancellationToken));
    }

    [Fact]
    public async Task SetUsersRoleAsync_RoleNotFound_ThrowsNotFoundException()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(UserId)).ReturnsAsync(TestUser);
        _roleManagerMock.Setup(r => r.FindByNameAsync(RoleName)).ReturnsAsync((ApplicationRole?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _rolesService.SetUsersRoleAsync(SetUsersRolesRequest, _cancellationToken));
    }
}
