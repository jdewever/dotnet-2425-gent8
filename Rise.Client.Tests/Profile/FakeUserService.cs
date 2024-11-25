using System;
using System.Threading.Tasks;
using Rise.Client.Auth;
using Rise.Shared.User;

namespace Rise.Client.Profile;

public class FakeUserService : IUserService
{
    private UserDto? _user;

    public FakeUserService()
    {
    }

    public Task<UserDto> GetCurrentUser()
    {
        _user = new UserDto
        {
            Email = "test@test.com",
            Picture = "test.png",
            FullName = "Test User",
            IsBlocked = false,
        };
        return Task.FromResult(_user);
    }

    public void ClearUser()
    {
        _user = null;
    }
}