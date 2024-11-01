namespace Events.Application.UseCases.Participants.DTOs;

public class EventParticipantResponseDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime RegistrationDate { get; set; }
    public Guid UserId { get; set; }
    public int EventId { get; set; }
}

