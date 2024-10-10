using AutoMapper;
using Events.Application.DTOs.Users.Responses;
using Events.Application.Interfaces;
using Events.Application.Services;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Events.Infrastructure.Data;
using Events.Tests.Application.TestBases;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Moq;

namespace Events.Tests.Application.Services.Tests;
public class UserDataServiceTests : UserDataServiceTestBase
{
    private readonly IUserDataService _userDataService;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly AppDbContext _context;

    public UserDataServiceTests()
    {
        _context = AppDbContextFactory.Create();
        _userManagerMock = CreateUserManager();
        var mapper = CreateMapper();
        _userDataService = new UserDataService(_userManagerMock.Object, mapper);
    }

    private Mock<UserManager<ApplicationUser>> CreateUserManager()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        var userStore = new UserStore<ApplicationUser>(_context);

        userManagerMock.Setup(m => m.Users).Returns(userStore.Users);

        return userManagerMock;
    }

    private IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ApplicationUser, UserDataResponse>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());
        });

        return config.CreateMapper();
    }

    [Fact]
    public async Task GetUserDataByUserIdAsync_ShouldReturnUserData_WhenUserExists()
    {
        _userManagerMock.Setup(um => um.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == User1Id)))
            .ReturnsAsync(UserRoles);

        var result = await _userDataService.GetUserDataByUserIdAsync(GetUserDataByUserIdRequest_Success, _cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(User1Name, result.UserName);
        Assert.Equal(User1Email, result.Email);
        Assert.Equal(UserRoles, result.Roles);
    }

    [Fact]
    public async Task GetUserDataByUserIdAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        _userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new List<string>());

        await Assert.ThrowsAsync<NotFoundException>(() => 
        _userDataService.GetUserDataByUserIdAsync(GetUserDataByUserIdRequest_Failure, _cancellationToken));
    }

    [Fact]
    public async Task GetUserDataAsync_ShouldReturnUserDataWithRoles_WhenUserExists()
    {
        _userManagerMock.Setup(um => um.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == User2Id)))
            .ReturnsAsync(new List<string> { "Admin" });

        var result = await _userDataService.GetUserDataAsync(User2Id, _cancellationToken);

        Assert.NotNull(result);
        Assert.Equal("testuser2", result.UserName);
        Assert.Equal("testuser2@example.com", result.Email);
        Assert.Single(result.Roles);
        Assert.Contains("Admin", result.Roles);
    }
}