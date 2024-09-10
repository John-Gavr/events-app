namespace Events.Core.Entities;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDateTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int MaxParticipants { get; set; }
    public bool IsFull { get; set; } = false;
    public List<EventParticipant> Participants { get; set; } = new List<EventParticipant>();
    public byte[]? Image { get; set; }
}
