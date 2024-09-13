using AutoMapper;
using Events.Application.DTOs.Roles.Requests;
using Events.Application.DTOs.Roles.Responses;
using Events.Application.Services;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Events.Tests.Application.Services;

public class RolesServiceTests : ApplicationTestBase
{
    private readonly RolesService _service;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<RoleManager<ApplicationRole>> _roleManagerMock;
    private readonly Mock<IMapper> _mapperMock;

    public RolesServiceTests()
    {
        _userManagerMock = MockUserManager<ApplicationUser>();
        _roleManagerMock = MockRoleManager<ApplicationRole>();
        _mapperMock = new Mock<IMapper>();

        _service = new RolesService(_roleManagerMock.Object, _userManagerMock.Object, _mapperMock.Object);
    }

    [Fact]
    public void GetAllRoles_ShouldReturnMappedRoles()
    {
        var roles = new List<ApplicationRole> { new ApplicationRole { Name = "Admin" } };
        var mappedRoles = new List<RoleResponse> { new RoleResponse { RoleName = "Admin" } };

        _roleManagerMock.Setup(r => r.Roles).Returns(roles.AsQueryable());
        _mapperMock.Setup(m => m.Map<RoleResponse>(It.IsAny<ApplicationRole>())).Returns(mappedRoles[0]);

        var result = _service.GetAllRoles();

        Assert.NotNull(result);
        Assert.Equal(mappedRoles.Count, result.Count);
        _roleManagerMock.Verify(r => r.Roles, Times.Once);
    }

    [Fact]
    public async Task GetUsersRoleAsync_ShouldReturnUserRoles_WhenUserExists()
    {
        var user = new ApplicationUser { Id = "1", UserName = "testUser" };
        var roles = new List<string> { "Admin" };
        var request = new GetUserRolesRequest { UserId = "1" };

        _userManagerMock.Setup(u => u.FindByIdAsync("1")).ReturnsAsync(user);
        _userManagerMock.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(roles);

        var result = await _service.GetUsersRoleAsync(request);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Admin", result[0].RoleName);
        _userManagerMock.Verify(u => u.FindByIdAsync("1"), Times.Once);
        _userManagerMock.Verify(u => u.GetRolesAsync(user), Times.Once);
    }

    [Fact]
    public async Task GetUsersRoleAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        var request = new GetUserRolesRequest { UserId = "1" };
        _userManagerMock.Setup(u => u.FindByIdAsync("1")).ReturnsAsync((ApplicationUser)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetUsersRoleAsync(request));
    }

    [Fact]
    public async Task SetUsersRoleAsync_ShouldAddRoleToUser_WhenUserAndRoleExist()
    {
        var user = new ApplicationUser { Id = "1", UserName = "testUser" };
        var role = new ApplicationRole { Name = "Admin" };
        var request = new SetUsersRolesRequest { UserId = "1", RoleName = "Admin" };

        _userManagerMock.Setup(u => u.FindByIdAsync("1")).ReturnsAsync(user);
        _roleManagerMock.Setup(r => r.FindByNameAsync("Admin")).ReturnsAsync(role);

        await _service.SetUsersRoleAsync(request);

        _userManagerMock.Verify(u => u.AddToRoleAsync(user, "Admin"), Times.Once);
    }

    [Fact]
    public async Task SetUsersRoleAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        var request = new SetUsersRolesRequest { UserId = "1", RoleName = "Admin" };
        _userManagerMock.Setup(u => u.FindByIdAsync("1")).ReturnsAsync((ApplicationUser)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.SetUsersRoleAsync(request));
    }

    [Fact]
    public async Task SetUsersRoleAsync_ShouldThrowNotFoundException_WhenRoleDoesNotExist()
    {
        var user = new ApplicationUser { Id = "1", UserName = "testUser" };
        var request = new SetUsersRolesRequest { UserId = "1", RoleName = "NonExistentRole" };

        _userManagerMock.Setup(u => u.FindByIdAsync("1")).ReturnsAsync(user);
        _roleManagerMock.Setup(r => r.FindByNameAsync("NonExistentRole")).ReturnsAsync((ApplicationRole)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.SetUsersRoleAsync(request));
    }

    private static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();
        return new Mock<UserManager<TUser>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
    }

    private static Mock<RoleManager<TRole>> MockRoleManager<TRole>() where TRole : class
    {
        var store = new Mock<IRoleStore<TRole>>();
        return new Mock<RoleManager<TRole>>(store.Object, null!, null!, null!, null!);
    }
}
