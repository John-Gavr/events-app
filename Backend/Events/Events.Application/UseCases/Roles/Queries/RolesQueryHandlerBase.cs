﻿using AutoMapper;
using Events.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.Roles.Queries;

public class RolesQueryHandlerBase
{
    protected readonly UserManager<ApplicationUser> _userManager;
    protected readonly RoleManager<ApplicationRole> _roleManager;
    protected readonly IMapper _mapper;
    public RolesQueryHandlerBase(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }
}