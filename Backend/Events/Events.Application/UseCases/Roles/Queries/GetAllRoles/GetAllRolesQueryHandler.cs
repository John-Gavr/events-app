using AutoMapper;
using Events.Application.UseCases.Roles.DTOs;
using Events.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.Roles.Queries.GetAllRoles;

public class GetAllRolesQueryHandler : RolesQueryHandlerBase, IRequestHandler<GetAllRolesQuery, IEnumerable<RoleNameResponseDTO>>
{
    public GetAllRolesQueryHandler(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
        IMapper mapper) : base(userManager, roleManager, mapper)
    { }

    public async Task<IEnumerable<RoleNameResponseDTO>> Handle(GetAllRolesQuery query, CancellationToken cancellationToken)
    {
        var roles = _roleManager.Roles;
        List<RoleNameResponseDTO> rolesList = [];
        foreach (var role in roles)
        {
            cancellationToken.ThrowIfCancellationRequested();
            rolesList.Add(_mapper.Map<RoleNameResponseDTO>(role));
        }
        return await Task.FromResult(rolesList);
    }
}
