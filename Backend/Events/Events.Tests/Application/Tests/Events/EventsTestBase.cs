using Events.Application.UseCases.Events.Commands.CreateEvent;
using Events.Application.UseCases.Events.Commands.DeleteEvent;
using Events.Application.UseCases.Events.Commands.UpdateEvent;
using Events.Application.UseCases.Events.Commands.UpdateEventsImage;
using Events.Application.UseCases.Events.DTOs;
using Events.Application.UseCases.Events.Queries.GetAllEvents;
using Events.Application.UseCases.Events.Queries.GetEventByCriteria;
using Events.Application.UseCases.Events.Queries.GetEventById;
using Events.Application.UseCases.Events.Queries.GetEventByName;
using Events.Application.UseCases.Events.Queries.GetUsersEvents;
using Events.Core.Interfaces;
using Moq;

namespace Events.Tests.Application.Tests.Events;

public class EventsTestBase : ApplicationTestBase
{
    protected readonly Mock<IEventRepository> _eventRepositoryMock;
    public EventsTestBase()
    {
        _eventRepositoryMock = new Mock<IEventRepository>();
    }
    protected static readonly CreateEventCommand createEventCommand = new CreateEventCommand { Name = "Test Event", Location = "Test Location", Category = "Test Category" };
    protected static readonly DeleteEventCommand deleteEventCommand = new DeleteEventCommand { Id = 1 };
    protected static readonly UpdateEventCommand updateEventCommand = new UpdateEventCommand { Id = 1, Name = "Updated Event", Location = "Updated Location", Category = "Updated Category" };
    protected static readonly UpdateEventImageCommand updateEventImageCommand = new UpdateEventImageCommand { EventId = 1, ImageBytes = new byte[] { 1, 2, 3 } };
    protected static readonly GetAllEventsQuery getAllEventsQuery = new GetAllEventsQuery { PageNumber = 1, PageSize = 10 };
    protected static readonly GetEventByIdQuery getEventByIdQuery = new GetEventByIdQuery { Id = 1 };
    protected static readonly EventsResponseDTO eventsResponseDTO = new EventsResponseDTO
    {
        Events = new List<EventResponseDTO>
            {
                new EventResponseDTO { Id = 1, Name = "Test Event", Location = "Test Location", Category = "Test Category" }
            },
        TotalCount = 1
    };
    protected static readonly EventResponseDTO eventResponseDTO = new EventResponseDTO { Id = 1, Name = "eventToFindByName" };
    protected static readonly GetEventByNameQuery getEventByNameQuery = new GetEventByNameQuery { Name = "eventToFindByName" };
    protected static readonly GetUsersEventsQuery getUsersEventsQuery = new GetUsersEventsQuery { PageNumber = 1, PageSize = 10, UserId = userId};
    protected static readonly EventsResponseDTO userEventsDTO = new EventsResponseDTO
    {
        Events = new List<EventResponseDTO> {
            new EventResponseDTO { Id = 1, Name = "event 1" },
            new EventResponseDTO { Id = 2, Name = "event 2" },
            new EventResponseDTO { Id = 3, Name = "event 3" }
        },
        TotalCount = 3
    };

    protected static readonly GetEventsByCriteriaQuery getEventsByCriteriaQuery_Category = new GetEventsByCriteriaQuery {  };

}
