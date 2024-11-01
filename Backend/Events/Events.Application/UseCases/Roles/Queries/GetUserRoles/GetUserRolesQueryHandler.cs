using AutoMapper;
using Events.Application.UseCases.Roles.DTOs;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.Roles.Queries.GetUserRoles;

public class GetUserRolesQueryHandler : RolesQueryHandlerBase, IRequestHandler<GetUserRolesQuery, IEnumerable<RoleNameResponseDTO>>
{
    public GetUserRolesQueryHandler(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IMapper mapper) : base(userManager, roleManager, mapper)
    { }

    public async Task<IEnumerable<RoleNameResponseDTO>> Handle(GetUserRolesQuery query, CancellationToken cancellationToken)
    {
        var userEntity = await _userManager.FindByIdAsync(query.UserId);
        if (userEntity == null)
            throw new NotFoundException(nameof(userEntity), query.UserId);

        var rolesEntity = await _userManager.GetRolesAsync(userEntity);
        List<RoleNameResponseDTO> rolesList = [];
        foreach (var role in rolesEntity)
        {
            cancellationToken.ThrowIfCancellationRequested();
            rolesList.Add(new RoleNameResponseDTO
            {
                RoleName = role
            });
        }
        return rolesList;
    }
}
