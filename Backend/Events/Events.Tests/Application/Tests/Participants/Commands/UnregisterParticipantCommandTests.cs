using Events.Core.Entities.Exceptions;
using Events.Core.Entities;
using Moq;
using Events.Application.UseCases.Participants.Commands.UnregisterParticipant;

namespace Events.Tests.Application.Tests.Participants.Commands;

public class UnregisterParticipantCommandTests : ParticipantsTestBase
{
    private readonly UnregisterParticipantCommandHandler _handler;

    public UnregisterParticipantCommandTests()
    {
        _handler = new(_mapperMock.Object, _eventParticipantRepositoryMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task UnregisterParticipantAsync_ShouldUnregisterParticipant_WhenValidRequest()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(testUser);
        _eventParticipantRepositoryMock.Setup(p => p.GetParticipantByUserIdAsync(userId, _cancellationToken)).ReturnsAsync(testParticipant);

        await _handler.Handle(unregisterParticipantCommand, _cancellationToken);

        _eventParticipantRepositoryMock.Verify(p => p.UnregisterParticipantAsync(unregisterParticipantCommand.EventId, userId, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UnregisterParticipantAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync((ApplicationUser)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(unregisterParticipantCommand, _cancellationToken));
    }

    [Fact]
    public async Task UnregisterParticipantAsync_ShouldThrowNotFoundException_WhenParticipantNotFound()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(testUser);
        _eventParticipantRepositoryMock.Setup(p => p.GetParticipantByUserIdAsync(userId, _cancellationToken)).ReturnsAsync((EventParticipant)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(unregisterParticipantCommand, _cancellationToken));
    }
}
