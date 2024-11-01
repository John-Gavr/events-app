using Events.Application.UseCases.Roles.DTOs;
using MediatR;

namespace Events.Application.UseCases.Roles.Queries.GetUserRoles;

public class GetUserRolesQuery : IRequest<IEnumerable<RoleNameResponseDTO>>
{
    public string UserId { get; set; } = string.Empty;
}
