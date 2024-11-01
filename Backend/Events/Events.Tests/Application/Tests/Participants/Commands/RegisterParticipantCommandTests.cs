using Events.Core.Entities.Exceptions;
using Events.Core.Entities;
using Moq;
using Events.Application.UseCases.Participants.Commands.RegisterParticipant;

namespace Events.Tests.Application.Tests.Participants.Commands;

public class RegisterParticipantCommandTests : ParticipantsTestBase
{
    private readonly RegisterParticipantCommandHandler _handler;

    public RegisterParticipantCommandTests()
    {
        _handler = new(_mapperMock.Object, _eventParticipantRepositoryMock.Object, _userManagerMock.Object, _eventRepositoryMock.Object);
    }

    [Fact]
    public async Task RegisterParticipantAsync_ShouldRegisterParticipant_WhenValidRequest()
    {
        _mapperMock.Setup(m => m.Map<EventParticipant>(registerParticipantCommand)).Returns(new EventParticipant());
        _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(testUser);
        _eventRepositoryMock.Setup(e => e.GetEventByIdAsync(registerParticipantCommand.EventId, _cancellationToken)).ReturnsAsync(testEvent);
        _eventParticipantRepositoryMock.Setup(p => p.RegisterParticipantAsync(registerParticipantCommand.EventId, It.IsAny<EventParticipant>(), _cancellationToken)).Returns(Task.CompletedTask);

        await _handler.Handle(registerParticipantCommand, _cancellationToken);

        _eventParticipantRepositoryMock.Verify(p => p.RegisterParticipantAsync(registerParticipantCommand.EventId, It.IsAny<EventParticipant>(), _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task RegisterParticipantAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        _mapperMock.Setup(m => m.Map<EventParticipant>(registerParticipantCommand)).Returns(new EventParticipant());
        _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync((ApplicationUser)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(registerParticipantCommand, _cancellationToken));
    }

    [Fact]
    public async Task RegisterParticipantAsync_ShouldThrowNotFoundException_WhenEventNotFound()
    {
        _mapperMock.Setup(m => m.Map<EventParticipant>(registerParticipantCommand)).Returns(new EventParticipant());
        _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(testUser);
        _eventRepositoryMock.Setup(e => e.GetEventByIdAsync(registerParticipantCommand.EventId, _cancellationToken)).ReturnsAsync((Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(registerParticipantCommand, _cancellationToken));
    }
}
