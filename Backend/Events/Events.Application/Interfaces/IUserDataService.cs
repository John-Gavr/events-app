using Events.Application.DTOs.Users.Requests.GetUserDataById;
using Events.Application.DTOs.Users.Responses;

namespace Events.Application.Interfaces;

public interface IUserDataService
{
    public Task<UserDataResponse> GetUserDataByUserIdAsync(GetUserDataByUserIdRequest request);
    public Task<UserDataResponse> GetUserDataAsync(string userId);
}
