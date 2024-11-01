using Events.Application.UseCases.Users.DTOs;
using MediatR;

namespace Events.Application.UseCases.Users.Queries.GetUserDataById;

public class GetUserDataByUserIdQuery : IRequest<UserDataResponseDTO>
{
    public string UserId { get; set; } = string.Empty;
}
