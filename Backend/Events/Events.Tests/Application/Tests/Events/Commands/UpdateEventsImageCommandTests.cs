using Events.Core.Entities.Exceptions;
using Events.Core.Entities;
using Moq;
using Events.Application.UseCases.Events.Commands.UpdateEventsImage;

namespace Events.Tests.Application.Tests.Events.Commands;
public class UpdateEventsImageCommandTests : EventsTestBase
{
    private readonly UpdateEventImageCommandHandler _handler;
    public UpdateEventsImageCommandTests()
    {
        _handler = new(_eventRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task UpdateEventsImageAsync_ShouldUpdateImage()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(testEvent);

        await _handler.Handle(updateEventImageCommand, _cancellationToken);

        _eventRepositoryMock.Verify(x => x.AddEventImageAsync(It.IsAny<int>(), It.IsAny<byte[]>(), _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateEventsImageAsync_ShouldThrowNotFoundException_WhenEventNotFound()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(updateEventImageCommand, _cancellationToken));
    }
}
