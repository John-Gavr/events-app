using Events.Application.UseCases.Roles.DTOs;
using MediatR;

namespace Events.Application.UseCases.Roles.Queries.GetAllRoles;

public class GetAllRolesQuery : IRequest<IEnumerable<RoleNameResponseDTO>>
{
}
