using Events.Application.DTOs.Participants.Requests.GetParticipantById;
using Events.Application.DTOs.Participants.Requests.GetParticipantsByEventId;
using Events.Application.DTOs.Participants.Requests.RegisterParticipant;
using Events.Application.DTOs.Participants.Requests.UnregisterParticipant;
using Events.Application.DTOs.Participants.Responses;
using Events.Application.Services;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Events.Core.Interfaces;
using Events.Tests.Application.TestBases;
using Moq;

namespace Events.Tests.Application.Services.Tests;

public class EventParticipantServiceTests : EventParticipantServiceTestBase
{
    private readonly Mock<IEventParticipantRepository> _eventParticipantRepositoryMock;
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly EventParticipantService _service;

    public EventParticipantServiceTests()
    {
        _eventParticipantRepositoryMock = new Mock<IEventParticipantRepository>();
        _eventRepositoryMock = new Mock<IEventRepository>();


        _service = new EventParticipantService(
            _eventParticipantRepositoryMock.Object,
            _eventRepositoryMock.Object,
            _mapperMock.Object,
            _userManagerMock.Object);
    }

    [Fact]
    public async Task RegisterParticipantAsync_ShouldRegisterParticipant_WhenValidRequest()
    {
        _mapperMock.Setup(m => m.Map<EventParticipant>(RegisterParticipantRequest)).Returns(new EventParticipant());
        _userManagerMock.Setup(u => u.FindByIdAsync(UserId)).ReturnsAsync(TestUser);
        _eventRepositoryMock.Setup(e => e.GetEventByIdAsync(RegisterParticipantRequest.EventId, _cancellationToken)).ReturnsAsync(TestEvent);
        _eventParticipantRepositoryMock.Setup(p => p.RegisterParticipantAsync(RegisterParticipantRequest.EventId, It.IsAny<EventParticipant>(), _cancellationToken)).Returns(Task.CompletedTask);

        await _service.RegisterParticipantAsync(RegisterParticipantRequest, UserId, _cancellationToken);

        _eventParticipantRepositoryMock.Verify(p => p.RegisterParticipantAsync(RegisterParticipantRequest.EventId, It.IsAny<EventParticipant>(), _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task RegisterParticipantAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        _mapperMock.Setup(m => m.Map<EventParticipant>(RegisterParticipantRequest)).Returns(new EventParticipant());
        _userManagerMock.Setup(u => u.FindByIdAsync(UserId)).ReturnsAsync((ApplicationUser)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.RegisterParticipantAsync(RegisterParticipantRequest, UserId, _cancellationToken));
    }

    [Fact]
    public async Task RegisterParticipantAsync_ShouldThrowNotFoundException_WhenEventNotFound()
    {
        _mapperMock.Setup(m => m.Map<EventParticipant>(RegisterParticipantRequest)).Returns(new EventParticipant());
        _userManagerMock.Setup(u => u.FindByIdAsync(UserId)).ReturnsAsync(TestUser);
        _eventRepositoryMock.Setup(e => e.GetEventByIdAsync(RegisterParticipantRequest.EventId, _cancellationToken)).ReturnsAsync((Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.RegisterParticipantAsync(RegisterParticipantRequest, UserId, _cancellationToken));
    }

    [Fact]
    public async Task GetParticipantsByEventIdAsync_ShouldReturnParticipants_WhenEventExists()
    {
        _eventRepositoryMock.Setup(e => e.GetEventByIdAsync(GetParticipantsByEventIdRequest.EventId, _cancellationToken)).ReturnsAsync(TestEvent);
        _eventParticipantRepositoryMock.Setup(p => p.GetParticipantsByEventIdAsync(GetParticipantsByEventIdRequest.EventId,
            GetParticipantsByEventIdRequest.PageNumber,
            GetParticipantsByEventIdRequest.PageSize,
            _cancellationToken)).ReturnsAsync(ParticipantsList);
        _mapperMock.Setup(m => m.Map<IEnumerable<EventParticipantResponse>>(ParticipantsList)).Returns(new List<EventParticipantResponse>());

        var result = await _service.GetParticipantsByEventIdAsync(GetParticipantsByEventIdRequest, _cancellationToken);

        Assert.NotNull(result);
        _mapperMock.Verify(m => m.Map<IEnumerable<EventParticipantResponse>>(ParticipantsList), Times.Once);
    }

    [Fact]
    public async Task GetParticipantsByEventIdAsync_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        _eventRepositoryMock.Setup(e =>
        e.GetEventByIdAsync(GetParticipantsByEventIdRequest.EventId, _cancellationToken)).ReturnsAsync((Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetParticipantsByEventIdAsync(GetParticipantsByEventIdRequest,
            _cancellationToken));
    }

    [Fact]
    public async Task GetParticipantByUserIdAsync_ShouldReturnParticipant_WhenUserAndParticipantExist()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(GetParticipantByUserIdRequest.UserId)).ReturnsAsync(TestUser);
        _eventParticipantRepositoryMock.Setup(p => p.GetParticipantByUserIdAsync(GetParticipantByUserIdRequest.UserId, _cancellationToken)).ReturnsAsync(TestParticipant);
        _mapperMock.Setup(m => m.Map<EventParticipantResponse>(TestParticipant)).Returns(new EventParticipantResponse());

        var result = await _service.GetParticipantByUserIdAsync(GetParticipantByUserIdRequest, _cancellationToken);

        Assert.NotNull(result);
        _mapperMock.Verify(m => m.Map<EventParticipantResponse>(TestParticipant), Times.Once);
    }

    [Fact]
    public async Task GetParticipantByUserIdAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(GetParticipantByUserIdRequest.UserId)).ReturnsAsync((ApplicationUser)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetParticipantByUserIdAsync(GetParticipantByUserIdRequest, _cancellationToken));
    }

    [Fact]
    public async Task UnregisterParticipantAsync_ShouldUnregisterParticipant_WhenValidRequest()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(UserId)).ReturnsAsync(TestUser);
        _eventParticipantRepositoryMock.Setup(p => p.GetParticipantByUserIdAsync(UserId, _cancellationToken)).ReturnsAsync(TestParticipant);

        await _service.UnregisterParticipantAsync(UnregisterParticipantRequest, UserId, _cancellationToken);

        _eventParticipantRepositoryMock.Verify(p => p.UnregisterParticipantAsync(UnregisterParticipantRequest.EventId, UserId, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UnregisterParticipantAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(UserId)).ReturnsAsync((ApplicationUser)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.UnregisterParticipantAsync(UnregisterParticipantRequest, UserId, _cancellationToken));
    }

    [Fact]
    public async Task UnregisterParticipantAsync_ShouldThrowNotFoundException_WhenParticipantNotFound()
    {
        _userManagerMock.Setup(u => u.FindByIdAsync(UserId)).ReturnsAsync(TestUser);
        _eventParticipantRepositoryMock.Setup(p => p.GetParticipantByUserIdAsync(UserId, _cancellationToken)).ReturnsAsync((EventParticipant)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.UnregisterParticipantAsync(UnregisterParticipantRequest, UserId, _cancellationToken));
    }
}
