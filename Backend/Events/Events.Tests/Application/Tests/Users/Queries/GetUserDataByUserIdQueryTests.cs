using Events.Application.UseCases.Users.DTOs;
using Events.Application.UseCases.Users.Queries.GetUserDataById;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Moq;

namespace Events.Tests.Application.Tests.Users.Queries;

public class GetUserDataByUserIdQueryTests : UsersTestBase
{
    private readonly GetUserDataByUserIdQueryHandler _handler;

    public GetUserDataByUserIdQueryTests()
    {
        _handler = new(_mapperMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task GetUserDataByUserIdAsync_ShouldReturnUserData_WhenUserExists()
    {
        _userManagerMock.Setup(um => um.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == user1Id)))
        .ReturnsAsync(userRoles);

        _mapperMock.Setup(m => m.Map<UserDataResponseDTO>(testUser1)).Returns(user1DataResponseDTO);

        var result = await _handler.Handle(getUserDataByUserIdRequest_Success, _cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(user1Name, result.UserName);
        Assert.Equal(user1Email, result.Email);
        Assert.Equal(userRoles, result.Roles);
    }

    [Fact]
    public async Task GetUserDataByUserIdAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        _userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new List<string>());

        await Assert.ThrowsAsync<NotFoundException>(() =>
        _handler.Handle(getUserDataByUserIdRequest_Failure, _cancellationToken));
    }
}
