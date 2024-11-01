using MediatR;

namespace Events.Application.UseCases.Participants.Commands.RegisterParticipant;

public class RegisterParticipantCommand : IRequest<Unit>
{
    public string UserId { get; set; } = string.Empty;
    public int EventId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string Email { get; set; } = string.Empty;
}
