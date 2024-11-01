using Events.Core.Entities.Exceptions;
using Events.Core.Entities;
using Moq;
using Events.Application.UseCases.Events.Commands.DeleteEvent;

namespace Events.Tests.Application.Tests.Events.Commands;

public class DeleteEventCommandTests : EventsTestBase
{
    private readonly DeleteEventCommandHandler _handler;
    public DeleteEventCommandTests()
    {
        _handler = new DeleteEventCommandHandler(_eventRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task DeleteEventAsync_ShouldDeleteEvent()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(testEvent);

        await _handler.Handle(deleteEventCommand, _cancellationToken);

        _eventRepositoryMock.Verify(x => x.DeleteEventAsync(1, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task DeleteEventAsync_ShouldThrowNotFoundException_WhenEventNotFound()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(deleteEventCommand, _cancellationToken));
    }
}
