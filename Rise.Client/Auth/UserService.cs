using System.Net.Http.Json;
using Rise.Shared.User;

namespace Rise.Client.Auth;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private UserDto? _user;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserDto> GetCurrentUser()
    {
        return _user ??= await _httpClient.GetFromJsonAsync<UserDto>("user/details") ??
                         throw new InvalidOperationException();
    }

    public void ClearUser()
    {
        _user = null;
    }
}