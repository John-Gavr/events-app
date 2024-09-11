using Events.Application.DTOs.Roles.Requests;
using Events.Application.DTOs.Roles.Responses;

namespace Events.Application.Interfaces;

public interface IRolesService
{
    public List<RoleResponse> GetAllRoles();
    public Task<List<RoleResponse>> GetUsersRoleAsync(GetUserRolesRequest request);

    public Task SetUsersRoleAsync(SetUsersRolesRequest request);
}
