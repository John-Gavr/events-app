using Events.Application.UseCases.Users.DTOs;
using Events.Application.UseCases.Users.Queries.GetUserData;
using Events.Core.Entities;
using Moq;

namespace Events.Tests.Application.Tests.Users.Queries;

public class GetCurrentUserDataQueryTests : UsersTestBase
{
    private readonly GetCurrentUserDataQueryHandler _handler;

    public GetCurrentUserDataQueryTests()
    {
        _handler = new(_mapperMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task GetUserDataAsync_ShouldReturnUserDataWithRoles_WhenUserExists()
    {
        _userManagerMock.Setup(um => um.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == user2Id)))
            .ReturnsAsync(new List<string> { "Admin" });
        _mapperMock.Setup(m => m.Map<UserDataResponseDTO>(testUser2)).Returns(user2DataResponseDTO);
        var result = await _handler.Handle(getCurrentUserDataQuery, _cancellationToken);

        Assert.NotNull(result);
        Assert.Equal("testuser2", result.UserName);
        Assert.Equal("testuser2@example.com", result.Email);
        Assert.Single(result.Roles);
        Assert.Contains("Admin", result.Roles);
    }
}
