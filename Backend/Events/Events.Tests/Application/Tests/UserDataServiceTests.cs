using AutoMapper;
using Events.Application.DTOs.Users.Requests.GetUserDataById;
using Events.Application.DTOs.Users.Responses;
using Events.Application.Services;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Events.Tests.Application.Extensions;

namespace Events.Tests.Application.Services
{
    public class UserDataServiceTests : ApplicationTestBase
    {
        private readonly UserDataService _service;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;

        public UserDataServiceTests()
        {
            _userManagerMock = MockUserManager<ApplicationUser>();
            _service = new UserDataService(_userManagerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetUserDataByUserIdAsync_ShouldReturnUserData_WhenUserExists()
        {
            var userId = Guid.NewGuid().ToString();
            // Arrange
            var request = new GetUserDataByUserIdRequest { UserId =  userId };
            var user = new ApplicationUser { Id = userId, UserName = "testUser" };
            var userDataResponse = new UserDataResponse { UserName = "testUser" };

            _userManagerMock.Setup(u => u.Users)
                .Returns(new List<ApplicationUser> { user }.AsQueryable().BuildMockDbSet().Object);
            _mapperMock.Setup(m => m.Map<UserDataResponse>(user)).Returns(userDataResponse);

            // Act
            var result = await _service.GetUserDataByUserIdAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("testUser", result.UserName);
            _userManagerMock.Verify(u => u.Users, Times.Once);
        }

        [Fact]
        public async Task GetUserDataByUserIdAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new GetUserDataByUserIdRequest { UserId = "1" };
            _userManagerMock.Setup(u => u.Users)
                .Returns(new List<ApplicationUser>().AsQueryable().BuildMockDbSet().Object);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetUserDataByUserIdAsync(request));
        }

        [Fact]
        public async Task GetUserDataAsync_ShouldReturnUserData_WhenUserExists()
        {
            // Arrange
            var userId = "1";
            var user = new ApplicationUser { Id = "1", UserName = "testUser" };
            var userDataResponse = new UserDataResponse { UserName = "testUser" };

            _userManagerMock.Setup(u => u.Users)
                .Returns(new List<ApplicationUser> { user }.AsQueryable().BuildMockDbSet().Object);
            _mapperMock.Setup(m => m.Map<UserDataResponse>(user)).Returns(userDataResponse);

            // Act
            var result = await _service.GetUserDataAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("testUser", result.UserName);
            _userManagerMock.Verify(u => u.Users, Times.Once);
        }

        [Fact]
        public async Task GetUserDataAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = "1";
            _userManagerMock.Setup(u => u.Users)
                .Returns(new List<ApplicationUser>().AsQueryable().BuildMockDbSet().Object);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetUserDataAsync(userId));
        }

        // Helper method to mock UserManager
        private static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        }
    }
}
