using Events.Application.DTOs.Users.Requests.GetUserDataByEmail;
using Events.Application.DTOs.Users.Requests.GetUserDataById;
using Events.Application.DTOs.Users.Responses;

namespace Events.Application.Interfaces;

public interface IUserDataService
{

    public Task<UserDataResponse> GetUserDataById(GetUserDataByIdRequest request);
    public Task<UserDataResponse> GetUserDataByEmail(GetUserDataByEmailRequest request);
}
