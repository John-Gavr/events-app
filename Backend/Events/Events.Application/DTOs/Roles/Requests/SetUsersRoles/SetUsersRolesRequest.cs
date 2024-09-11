namespace Events.Application.DTOs.Roles.Requests;

public class SetUsersRolesRequest
{
    public string UserId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
}
