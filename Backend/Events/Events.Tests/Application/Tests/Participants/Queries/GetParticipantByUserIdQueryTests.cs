using Events.Application.UseCases.Participants.DTOs;
using Events.Core.Entities.Exceptions;
using Events.Core.Entities;
using Moq;
using Events.Application.UseCases.Participants.Queries.GetParticipantByUserId;

namespace Events.Tests.Application.Tests.Participants.Queries;

public class GetParticipantByUserIdQueryTests : ParticipantsTestBase
{
    private readonly GetParticipantByUserIdQueryHandler _handler;

    public GetParticipantByUserIdQueryTests()
    {
        _handler = new(_mapperMock.Object, _eventParticipantRepositoryMock.Object, _userManagerMock.Object);
    }
    [Fact]
    public async Task GetParticipantByUserIdAsync_ShouldReturnParticipant_WhenUserAndParticipantExist()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(getParticipantByUserIdQuery.UserId)).ReturnsAsync(testUser);
        _eventParticipantRepositoryMock.Setup(p => 
        p.GetParticipantByUserIdAsync(getParticipantByUserIdQuery.UserId, _cancellationToken)).ReturnsAsync(testParticipant);
        _mapperMock.Setup(m => m.Map<EventParticipantResponseDTO>(testParticipant)).Returns(new EventParticipantResponseDTO());

        var result = await _handler.Handle(getParticipantByUserIdQuery, _cancellationToken);

        Assert.NotNull(result);
        _mapperMock.Verify(m => m.Map<EventParticipantResponseDTO>(testParticipant), Times.Once);
    }

    [Fact]
    public async Task GetParticipantByUserIdAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(getParticipantByUserIdQuery.UserId)).ReturnsAsync((ApplicationUser)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(getParticipantByUserIdQuery, _cancellationToken));
    }
}
