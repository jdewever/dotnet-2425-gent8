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

    public async Task<UserDTO> AddUser(UserCreationDTO user)
    {
        var response = await httpClient.PostAsJsonAsync("user", user);
        response.EnsureSuccessStatusCode();
        var u = await response.Content.ReadFromJsonAsync<UserDTO>();
        return u ?? throw new Exception("Failed to create user");
    }

    public async Task<UserDTO> BlockUser(string userId)
    {
        var response = await httpClient.PostAsync($"user/{userId}/block", null);
        response.EnsureSuccessStatusCode();
        var user = await response.Content.ReadFromJsonAsync<UserDTO>();
        return user!;
    }

    public async Task<bool> DeleteUser(string userId)
    {
        var response = await httpClient.DeleteAsync($"user/{userId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<UserDTO> GetUser(string userId, bool forceRefresh = false)
    {
        var user = await httpClient.GetFromJsonAsync<UserDTO>($"user/detail");
        return user!;
    }

    public async Task<IEnumerable<UserDTO>> GetUsers(bool forceRefresh = false)
    {
        var response = await httpClient.GetFromJsonAsync<IEnumerable<UserDTO>>("user");
        return response ?? Array.Empty<UserDTO>();
    }
}