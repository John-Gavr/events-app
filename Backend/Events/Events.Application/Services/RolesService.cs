using Microsoft.AspNetCore.Identity;
using Events.Application.Interfaces;
using Events.Core.Entities;
using Events.Application.DTOs.Roles.Responses;
using AutoMapper;
using Events.Core.Entities.Exceptions;
using Events.Application.DTOs.Roles.Requests;

namespace Events.Application.Services;

public class RolesService : IRolesService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IMapper _mapper;
    public RolesService(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _mapper = mapper;
    }
    public List<RoleResponse> GetAllRoles(CancellationToken cancellationToken)
    {
        var roles = _roleManager.Roles;
        List<RoleResponse> rolesList = [];
        foreach(var role in roles)
        {
            cancellationToken.ThrowIfCancellationRequested();
            rolesList.Add(_mapper.Map<RoleResponse>(role));
        }
        return rolesList;   
    }

    public async Task<List<RoleNameResponse>> GetUsersRoleAsync(GetUserRolesRequest request, CancellationToken cancellationToken)
    {
        var userEntity = await _userManager.FindByIdAsync(request.UserId);
        if (userEntity == null)
            throw new NotFoundException(nameof(userEntity), request.UserId);
        
        var rolesEntity = await _userManager.GetRolesAsync(userEntity);
        List<RoleNameResponse> rolesList = [];
        foreach (var role in rolesEntity)
        {
            cancellationToken.ThrowIfCancellationRequested();
            rolesList.Add(new RoleNameResponse { 
                RoleName = role
            });
        }
        return rolesList;
    }

    public async Task SetUsersRoleAsync(SetUsersRolesRequest request, CancellationToken cancellationToken)
    {
        var userEntity = await _userManager.FindByIdAsync(request.UserId);
        if (userEntity == null)
            throw new NotFoundException(nameof(userEntity), request.UserId);

        var role = await _roleManager.FindByNameAsync(request.RoleName);
        if (role == null)
            throw new NotFoundException(nameof(role), request.RoleName);

        cancellationToken.ThrowIfCancellationRequested();

        await _userManager.AddToRoleAsync(userEntity, request.RoleName);
    }
}
