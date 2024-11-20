using System.Net.Http.Json;
using Rise.Shared.User;

namespace Rise.Client.Auth;

public class UserService
{
    private readonly HttpClient _httpClient;
    
    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserDto> GetCurrentUser()
    {
        var response = await _httpClient.GetFromJsonAsync<UserDto>("user/details");
        return response!;
    }
}