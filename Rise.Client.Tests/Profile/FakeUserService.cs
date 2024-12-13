using System.Threading.Tasks;
using Rise.Client.Auth;
using Rise.Shared.User;

namespace Rise.Client.Profile;

public class FakeUserService : ILocalUserService
{
    private UserDTO? _user;

    public FakeUserService()
    {
    }

    public Task<UserDTO> GetCurrentUser()
    {
        _user = new UserDTO
        {
            UserID = "test",
            Email = "test@test.com",
            Picture = "test.png",
            FullName = "Test User",
            IsBlocked = false,
            Role = "Administrator"
        };
        return Task.FromResult(_user);
    }

    public void ClearUser()
    {
        _user = null;
    }
}