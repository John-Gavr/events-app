using Events.Core.Entities.Exceptions;
using Events.Core.Entities;
using Moq;
using Events.Application.UseCases.Events.Commands.UpdateEvent;

namespace Events.Tests.Application.Tests.Events.Commands;

public class UpdateEventCommandTests : EventsTestBase
{

    private readonly UpdateEventCommandHandler _handler;

    public UpdateEventCommandTests()
    {
        _handler = new(_eventRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task UpdateEventAsync_ShouldUpdateEvent()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(testEvent);

        await _handler.Handle(updateEventCommand, _cancellationToken);

        _eventRepositoryMock.Verify(x => x.UpdateEventAsync(It.IsAny<Event>(), _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateEventAsync_ShouldThrowNotFoundException_WhenEventNotFound()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(updateEventCommand, _cancellationToken));
    }

}
