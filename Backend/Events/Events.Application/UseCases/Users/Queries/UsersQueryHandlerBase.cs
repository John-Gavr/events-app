using AutoMapper;
using Events.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.Users.Queries;

public class UsersQueryHandlerBase
{
    protected readonly UserManager<ApplicationUser> _userManager;
    protected readonly IMapper _mapper;

    public UsersQueryHandlerBase(IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }
}
