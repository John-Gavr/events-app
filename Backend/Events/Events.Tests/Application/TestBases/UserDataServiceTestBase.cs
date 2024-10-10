using Events.Application.DTOs.Users.Requests.GetUserDataById;
using Events.Core.Entities;

namespace Events.Tests.Application.TestBases;
public class UserDataServiceTestBase
{
    protected static readonly CancellationToken _cancellationToken = CancellationToken.None;
    protected static readonly string User1Id = "96669AE7-8DC5-4034-917C-CAF6B606EEF8";
    protected static readonly string User2Id = "510964E5-C933-44C1-AE0D-A9830A6D0ED5";
    protected static readonly string User1Name = "testuser1";
    protected static readonly string User2Name = "testuser2";
    protected static readonly string User1Email = "testuser1@example.com";
    protected static readonly string User2Email = "testuser2@example.com";
    protected readonly GetUserDataByUserIdRequest GetUserDataByUserIdRequest_Success = new GetUserDataByUserIdRequest { UserId = User1Id };
    protected readonly GetUserDataByUserIdRequest GetUserDataByUserIdRequest_Failure = new GetUserDataByUserIdRequest 
    { 
        UserId = "96669AE7-8DC5-4034-917C-CAF6B606E000" 
    };
    public static List<ApplicationUser> Users => new List<ApplicationUser>
    {
        new ApplicationUser { Id = User1Id, UserName = User1Name, Email = User1Email },
        new ApplicationUser { Id = User2Id, UserName = User2Name, Email = User2Email }
    };
    public static List<string> UserRoles => new List<string>
    {
        "User",
        "Admin"
    };
}
