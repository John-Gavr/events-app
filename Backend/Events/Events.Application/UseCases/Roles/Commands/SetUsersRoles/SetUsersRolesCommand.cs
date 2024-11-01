using MediatR;

namespace Events.Application.UseCases.Roles.Commands.SetUsersRoles;

public class SetUsersRolesCommand : IRequest<Unit>
{
    public string UserId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
}
