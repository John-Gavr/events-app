using Events.Application.DTOs.Roles.Requests;
using Events.Application.DTOs.Roles.Responses;

namespace Events.Application.Interfaces;

public interface IRolesService
{
    public List<RoleResponse> GetAllRoles(CancellationToken cancellationToken);
    public Task<List<RoleNameResponse>> GetUsersRoleAsync(GetUserRolesRequest request, CancellationToken cancellationToken);
    public Task SetUsersRoleAsync(SetUsersRolesRequest request, CancellationToken cancellationToken);
}
