using AutoMapper;
using Events.Application.UseCases.Users.DTOs;
using Events.Application.UseCases.Users.Queries.GetUserData;
using Events.Application.UseCases.Users.Queries.GetUserDataById;
using Events.Core.Entities;
using Events.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Moq;

namespace Events.Tests.Application.Tests.Users;

public class UsersTestBase : ApplicationTestBase
{
    protected new readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    protected readonly AppDbContext _context;
    public UsersTestBase()
    {
        _context = AppDbContextFactory.Create();
        _userManagerMock = CreateUserManager();
        var mapper = CreateMapper();
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
            cfg.CreateMap<ApplicationUser, UserDataResponseDTO>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());
        });

        return config.CreateMapper();
    }

    protected static readonly string user1Id = "96669AE7-8DC5-4034-917C-CAF6B606EEF8";
    protected static readonly string user2Id = "510964E5-C933-44C1-AE0D-A9830A6D0ED5";
    protected static readonly string user1Name = "testuser1";
    protected static readonly string user2Name = "testuser2";
    protected static readonly string user1Email = "testuser1@example.com";
    protected static readonly string user2Email = "testuser2@example.com";
    protected readonly GetUserDataByUserIdQuery getUserDataByUserIdRequest_Success = new GetUserDataByUserIdQuery { UserId = user1Id };
    protected readonly GetUserDataByUserIdQuery getUserDataByUserIdRequest_Failure = new GetUserDataByUserIdQuery
    {
        UserId = "96669AE7-8DC5-4034-917C-CAF6B606E000"
    };
    protected readonly GetCurrentUserDataQuery getCurrentUserDataQuery = new GetCurrentUserDataQuery { UserId = user2Id };

    public static ApplicationUser testUser1 = new ApplicationUser() { Id = user1Id, UserName = user1Name, Email = user1Email };
    public static ApplicationUser testUser2 = new ApplicationUser() { Id = user2Id, UserName = user2Name, Email = user2Email };
    public static List<ApplicationUser> Users => [ testUser1, testUser2 ];

    protected static UserDataResponseDTO user2DataResponseDTO = new UserDataResponseDTO { UserName = user2Name, Email = user2Email };
    protected static UserDataResponseDTO user1DataResponseDTO = new UserDataResponseDTO { UserName = user1Name, Email = user1Email };
}
