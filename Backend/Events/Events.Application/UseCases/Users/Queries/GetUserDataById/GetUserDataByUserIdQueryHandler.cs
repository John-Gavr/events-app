using AutoMapper;
using Events.Application.UseCases.Users.DTOs;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Events.Application.UseCases.Users.Queries.GetUserDataById;

public class GetUserDataByUserIdQueryHandler : UsersQueryHandlerBase, IRequestHandler<GetUserDataByUserIdQuery, UserDataResponseDTO>
{
    public GetUserDataByUserIdQueryHandler(IMapper mapper, UserManager<ApplicationUser> userManager) : base(mapper, userManager)
    { }

    public async Task<UserDataResponseDTO> Handle(GetUserDataByUserIdQuery query, CancellationToken cancellationToken)
    {
        var userEntity = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);
        if (userEntity == null)
            throw new NotFoundException(nameof(userEntity), query.UserId);

        var roles = await _userManager.GetRolesAsync(userEntity);

        var userDataResponse = _mapper.Map<UserDataResponseDTO>(userEntity);
        userDataResponse.Roles = roles.ToList();

        return userDataResponse;
    }
}
