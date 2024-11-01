using Events.Core.Entities.Exceptions;
using Events.Core.Entities;
using Moq;
using Events.Application.UseCases.Events.Commands.CreateEvent;

namespace Events.Tests.Application.Tests.Events.Commands;

public class CreateEventCommandTests : EventsTestBase
{
    private readonly CreateEventCommandHandler _handler;
    public CreateEventCommandTests() {
        _handler = new(_eventRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task AddEventAsync_ShouldAddEvent()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null);
        _mapperMock.Setup(x => x.Map<Event>(It.IsAny<CreateEventCommand>()))
            .Returns(testEvent);

        await _handler.Handle(createEventCommand, _cancellationToken);

        _eventRepositoryMock.Verify(x => x.AddEventAsync(It.IsAny<Event>(), _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task AddEventAsync_ShouldThrowEventAlredyExistException_WhenEventAlreadyExists()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(testEvent);

        await Assert.ThrowsAsync<EventAlredyExistException>(() => _handler.Handle(createEventCommand, _cancellationToken));
    }


}
