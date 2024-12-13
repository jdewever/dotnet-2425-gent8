using Rise.Shared.User;

namespace Rise.Client.Auth;

public interface ILocalUserService
{
    public Task<UserDTO> GetCurrentUser();
    public void ClearUser();
}