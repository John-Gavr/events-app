using AutoMapper;
using Events.Application.DTOs.Users.Requests.GetUserDataById;
using Events.Application.DTOs.Users.Responses;
using Events.Application.Interfaces;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Events.Application.Services;

public class UserDataService : IUserDataService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public UserDataService(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserDataResponse> GetUserDataByUserIdAsync(GetUserDataByUserIdRequest request)
    {
        var userEntity = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId);
        if (userEntity == null)
            throw new NotFoundException(nameof(userEntity), request.UserId);

        var roles = await _userManager.GetRolesAsync(userEntity);

        var userDataResponse = _mapper.Map<UserDataResponse>(userEntity);
        userDataResponse.Roles = roles.ToList();

        return userDataResponse;
    }

    public async Task<UserDataResponse> GetUserDataAsync(string userId)
    {
        var userEntity = await _userManager.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();
        if (userEntity == null)
            throw new NotFoundException(nameof(userEntity), userId);

        var roles = await _userManager.GetRolesAsync(userEntity);

        var userDataResponse = _mapper.Map<UserDataResponse>(userEntity);
        userDataResponse.Roles = roles.ToList();

        return userDataResponse;
    }
}
