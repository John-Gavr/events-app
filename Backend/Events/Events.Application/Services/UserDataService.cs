using AutoMapper;
using Events.Application.DTOs.Users.Requests.GetUserDataByEmail;
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

    public async Task<UserDataResponse> GetUserDataById(GetUserDataByIdRequest request)
    {
        var user =  await _userManager.Users
            .Where(u => u.Id == request.UserId)
            .FirstOrDefaultAsync();
        if (user == null)
            throw new NotFoundException(nameof(user), request.UserId);
        return _mapper.Map<UserDataResponse>(user);
    }

    public async Task<UserDataResponse> GetUserDataByEmail(GetUserDataByEmailRequest request)
    {

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email!.Equals(request.Email));
        if(user == null) 
            throw new NotFoundException(nameof(user), request.Email);

        return _mapper.Map<UserDataResponse>(user);
    }
}
