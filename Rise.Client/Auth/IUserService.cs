using Rise.Shared.User;

namespace Rise.Client.Auth;

public interface IUserService
{
    public Task<UserDto> GetCurrentUser();
    public void ClearUser();
}