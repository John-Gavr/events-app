using Events.Application.DTOs.Events.Requests.CreateEvent;
using Events.Application.DTOs.Events.Requests.UpdateEvent;
using Events.Application.DTOs.Events.Requests.UpdateEventsImage;
using Events.Application.DTOs.Events.Responces;

namespace Events.Tests.Application.TestBases;
public class EventServiceTestBase : ApplicationTestBase
{
    protected static readonly CreateEventRequest CreateEventRequest = new CreateEventRequest { Name = "Test Event", Location = "Test Location", Category = "Test Category" };
    protected static readonly UpdateEventRequest UpdateEventRequest = new UpdateEventRequest { Name = "Updated Event", Location = "Updated Location", Category = "Updated Category" };
    protected static readonly UpdateEventImageRequest UpdateEventImageRequest = new UpdateEventImageRequest { EventId = 1, ImageBytes = new byte[] { 1, 2, 3 } };
    protected static readonly EventsResponse EventsResponse = new EventsResponse
    {
        Events = new List<EventResponse>
            {
                new EventResponse { Id = 1, Name = "Test Event", Location = "Test Location", Category = "Test Category" }
            },
        TotalCount = 1
    };
}
