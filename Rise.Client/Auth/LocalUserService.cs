using System.Net.Http.Json;
using Rise.Shared.User;

namespace Rise.Client.Auth;

public class LocalUserService : ILocalUserService
{
    private readonly HttpClient _httpClient;
    private UserDTO? _user;

    public LocalUserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserDTO> GetCurrentUser()
    {
        return _user ??= await _httpClient.GetFromJsonAsync<UserDTO>("user/details") ??
                         throw new InvalidOperationException();
    }

    public void ClearUser()
    {
        _user = null;
    }
}