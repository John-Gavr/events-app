using AutoMapper;
using Events.Application.DTOs.Users.Requests.GetUserDataById;
using Events.Application.DTOs.Users.Responses;
using Events.Application.Services;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Events.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Events.Application.Tests.Services;

public class UserDataServiceTests
{
    private readonly UserDataService _userDataService;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly IMapper _mapper;
    private readonly List<ApplicationUser> _users;

    private string User1Id = Guid.NewGuid().ToString();
    private string User2Id = Guid.NewGuid().ToString();
    private string NoSuchUserId = Guid.NewGuid().ToString();
    public UserDataServiceTests()
    { 
        _users = new List<ApplicationUser>
        {
            new ApplicationUser { Id = User1Id, UserName = "testuser1" },
            new ApplicationUser { Id = User2Id, UserName = "testuser2" }
        };

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var context = new AppDbContext(options);
        context.Users.AddRange(_users);
        context.SaveChanges();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ApplicationUser, UserDataResponse>();
        });

        _mapper = config.CreateMapper();

        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            new Mock<IUserStore<ApplicationUser>>().Object,
            null!, null!, null!, null!, null!, null!, null!, null!);

        _userManagerMock.Setup(um => um.Users).Returns(context.Users);

        _userDataService = new UserDataService(_userManagerMock.Object, _mapper);
    }

    [Fact]
    public async Task GetUserDataByUserIdAsync_UserExists_ShouldReturnUserDataResponse()
    {
        var request = new GetUserDataByUserIdRequest { UserId = User1Id };

        var result = await _userDataService.GetUserDataByUserIdAsync(request);

        Assert.NotNull(result);
        Assert.Equal(User1Id, result.Id);
        Assert.Equal(User1Id, result.Id);
    }

    [Fact]
    public async Task GetUserDataByUserIdAsync_UserDoesNotExist_ShouldThrowNotFoundException()
    {
        var request = new GetUserDataByUserIdRequest { UserId = NoSuchUserId };

        await Assert.ThrowsAsync<NotFoundException>(() => _userDataService.GetUserDataByUserIdAsync(request));
    }

    [Fact]
    public async Task GetUserDataAsync_UserExists_ShouldReturnUserDataResponse()
    {
        var result = await _userDataService.GetUserDataAsync(User1Id);

        Assert.NotNull(result);
        Assert.Equal(User1Id, result.Id);
    }

    [Fact]
    public async Task GetUserDataAsync_UserDoesNotExist_ShouldThrowNotFoundException()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _userDataService.GetUserDataAsync(NoSuchUserId));
    }
}