using Events.Application.UseCases.Users.DTOs;
using MediatR;

namespace Events.Application.UseCases.Users.Queries.GetUserData;

public class GetCurrentUserDataQuery : IRequest<UserDataResponseDTO>
{ 
    public string UserId { get; set; } = string.Empty;
}
