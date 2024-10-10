using AutoMapper;
using Events.Application.DTOs.Participants.Responses;
using Events.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Events.Tests.Application;
public class ApplicationTestBase
{
    protected readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    protected readonly CancellationToken _cancellationToken = CancellationToken.None;
    protected readonly Mock<IMapper> _mapperMock;
    public ApplicationTestBase()
    {
        _mapperMock = new Mock<IMapper>();
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
    }

    protected static readonly string UserId = Guid.NewGuid().ToString();
    protected static readonly ApplicationUser TestUser = new ApplicationUser { Id = UserId };
    protected static readonly List<EventParticipant> ParticipantsList = new List<EventParticipant>();
    protected static readonly IEnumerable<EventParticipantResponse> ParticipantResponses = new List<EventParticipantResponse>();
    protected static readonly Event TestEvent = new Event 
    { 
        Id = 1,
        Name = "Test Event",
        Location = "Test Location",
        Category = "Test Category", 
        Participants = new List<EventParticipant>() 
    };
    protected static readonly string RoleName = "Admin";
    protected static readonly ApplicationRole TestRole = new ApplicationRole { Name = RoleName };
    protected static readonly List<ApplicationRole> RolesList = new List<ApplicationRole> { TestRole };
    protected static readonly List<string> UserRoles = new List<string> 
    { 
        "Admin", 
        "User" 
    };
}
