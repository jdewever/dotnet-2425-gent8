using System.Net.Http.Json;
using Rise.Shared.User;

namespace Rise.Client.Manage;

public class UserService : IUserService
{
    private readonly HttpClient httpClient;

    public UserService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<UserDTO> BlockUser(string userId)
    {
        var response = await httpClient.PostAsync($"user/{userId}/block", null);
        response.EnsureSuccessStatusCode();
        var user = await response.Content.ReadFromJsonAsync<UserDTO>();
        return user!;
    }

    public async Task<UserDTO> GetUser(string userId)
    {
        var user = await httpClient.GetFromJsonAsync<UserDTO>($"user/detail");
        return user!;
    }

    public async Task<IEnumerable<UserDTO>> GetUsers()
    {
        var response = await httpClient.GetFromJsonAsync<IEnumerable<UserDTO>>("user");
        return response ?? Array.Empty<UserDTO>();
    }
}