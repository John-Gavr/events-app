using Events.Application.UseCases.Events.DTOs;
using Events.Application.UseCases.Events.Queries.GetUsersEvents;
using Events.Core.Entities.Exceptions;
using Events.Core.Entities;
using Moq;

namespace Events.Tests.Application.Tests.Events.Queries;

public class GetUsersEventsQueryTests : EventsTestBase
{
    private readonly GetUsersEventsQueryHandler _handler;

    public GetUsersEventsQueryTests()
    {
        _handler = new(_mapperMock.Object, _eventRepositoryMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task GetEventByNameAsync_ShouldReturnEvents()
    {
        _eventRepositoryMock.Setup(x => x.GetEventsByUserIdAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Event> {  new Event(), new Event(), new Event()});

        _userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(testUser);

        _mapperMock.Setup(x => x.Map<IEnumerable<EventResponseDTO>>(It.IsAny<IEnumerable<Event>>()))
        .Returns(userEventsDTO.Events);

        var result = await _handler.Handle(getUsersEventsQuery, _cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(3, result.Events.Count());
    }
    [Fact]
    public async Task GetEventByNameAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        _userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(getUsersEventsQuery, _cancellationToken));
    }
}
