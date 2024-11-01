namespace Events.Application.UseCases.Roles.DTOs;

public class RoleResponseDTO
{
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}
