namespace Events.Core.Entities;

public class EventParticipant
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string Email { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public int EventId { get; set; }
}