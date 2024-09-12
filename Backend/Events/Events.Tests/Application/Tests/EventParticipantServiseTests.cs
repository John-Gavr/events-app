using AutoMapper;
using Events.Application.DTOs.Participants.Requests.GetParticipantById;
using Events.Application.DTOs.Participants.Requests.GetParticipantsByEventId;
using Events.Application.DTOs.Participants.Requests.RegisterParticipant;
using Events.Application.DTOs.Participants.Requests.UnregisterParticipant;
using Events.Application.DTOs.Participants.Responses;
using Events.Application.Services;
using Events.Core.Entities;
using Events.Core.Interfaces;
using Moq;

namespace Events.Tests.Application.Services
{
    public class EventParticipantServiceTests : ApplicationTestBase
    {
        private readonly EventParticipantService _service;
        private readonly Mock<IEventParticipantRepository> _participantRepositoryMock;

        public EventParticipantServiceTests()
        {
            _participantRepositoryMock = new Mock<IEventParticipantRepository>();
            _service = new EventParticipantService(_participantRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task RegisterParticipantAsync_ShouldCallRepositoryWithMappedEntity()
        {
            var request = new RegisterParticipantRequest
            {
                EventId = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = System.DateTime.Now,
                Email = "john@example.com"
            };
            var participant = new EventParticipant { };
            var userId = Guid.NewGuid().ToString();

            _mapperMock.Setup(m => m.Map<EventParticipant>(request)).Returns(participant);

            await _service.RegisterParticipantAsync(request, userId);

            _participantRepositoryMock.Verify(r => r.RegisterParticipantAsync(request.EventId, participant), Times.Once);
            Assert.Equal(Guid.Parse(userId), participant.UserId);
        }

        [Fact]
        public async Task GetParticipantsByEventIdAsync_ShouldReturnMappedParticipants()
        {
            var request = new GetParticipantsByEventIdRequest { EventId = 1, PageNumber = 1, PageSize = 10 };
            var participants = new List<EventParticipant> { };
            var mappedParticipants = new List<EventParticipantResponse> { };

            _participantRepositoryMock.Setup(r => r.GetParticipantsByEventIdAsync(request.EventId, request.PageNumber, request.PageSize))
                                      .ReturnsAsync(participants);

            _mapperMock.Setup(m => m.Map<IEnumerable<EventParticipantResponse>>(participants))
                       .Returns(mappedParticipants);

            var result = await _service.GetParticipantsByEventIdAsync(request);

            Assert.NotNull(result);
            Assert.Equal(mappedParticipants.Count(), result.Count());
            _participantRepositoryMock.Verify(r => r.GetParticipantsByEventIdAsync(request.EventId, request.PageNumber, request.PageSize), Times.Once);
        }

        [Fact]
        public async Task GetParticipantByUserIdAsync_ShouldReturnMappedParticipant()
        {
            var request = new GetParticipantByUserIdRequest { UserId = Guid.NewGuid().ToString() };
            var participant = new EventParticipant { };
            var mappedParticipant = new EventParticipantResponse { };

            _participantRepositoryMock.Setup(r => r.GetParticipantByUserIdAsync(request.UserId))
                                      .ReturnsAsync(participant);

            _mapperMock.Setup(m => m.Map<EventParticipantResponse>(participant))
                       .Returns(mappedParticipant);

            var result = await _service.GetParticipantByUserIdAsync(request);

            Assert.NotNull(result);
            Assert.Equal(mappedParticipant.FirstName, result.FirstName);
            _participantRepositoryMock.Verify(r => r.GetParticipantByUserIdAsync(request.UserId), Times.Once);
        }

        [Fact]
        public async Task UnregisterParticipantAsync_ShouldCallRepositoryWithCorrectParameters()
        {
            var request = new UnregisterParticipantRequest { EventId = 1 };
            var userId = Guid.NewGuid().ToString();

            await _service.UnregisterParticipantAsync(request, userId);

            _participantRepositoryMock.Verify(r => r.UnregisterParticipantAsync(request.EventId, userId), Times.Once);
        }
    }
}
