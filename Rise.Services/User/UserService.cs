using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using Microsoft.Extensions.Caching.Hybrid;
using Rise.Services.Auth;
using Rise.Shared.User;

namespace Rise.Services.User;

public class UserService : IUserService
{
    private readonly IManagementApiClient managementApiClient;
    private readonly HybridCache _cache;
    private readonly IAuthContextProvider _authContextProvider;

    public UserService(IManagementApiClient managementApiClient, HybridCache cache, IAuthContextProvider authContextProvider)
    {
        this.managementApiClient = managementApiClient;
        _cache = cache;
        _authContextProvider = authContextProvider;
    }

    private async Task<IEnumerable<UserDTO>> GetUsersFromAuth0()
    {
        var rawUsers = await managementApiClient.Users.GetAllAsync(new GetUsersRequest(), new PaginationInfo());
        return await Task.WhenAll(rawUsers.Select(async user => new UserDTO
        {
            UserID = user.UserId,
            Email = user.Email,
            IsBlocked = user.Blocked ?? false,
            FullName = user.FullName,
            Picture = user.Picture,
            Role = await GetRole(user.UserId)
        }));
    }

    public async Task<IEnumerable<UserDTO>> GetUsers(bool forceRefresh = false)
    {
        if (forceRefresh)
            await _cache.RemoveAsync($"GET-USERINFO-ALL");

        return await _cache.GetOrCreateAsync(
            $"GET-USERINFO-ALL", // Unique key for the cache
            async cancel => await GetUsersFromAuth0(),
            cancellationToken: new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token
        );
    }

    private async Task<UserDTO> GetUserFromAuth0(string userId)
    {
        var user = await managementApiClient.Users.GetAsync(userId);
        return new UserDTO
        {
            UserID = user.UserId,
            Email = user.Email,
            IsBlocked = user.Blocked ?? false,
            FullName = user.FullName,
            Picture = user.Picture,
            Role = await GetRole(user.UserId)
        };
    }

    public async Task<UserDTO> GetUser(string userId, bool forceRefresh = false)
    {
        if (forceRefresh)
            await _cache.RemoveAsync($"GET-USERINFO-{userId}");

        return await _cache.GetOrCreateAsync(
            $"GET-USERINFO-{userId}", // Unique key for the cache
            async cancel => await GetUserFromAuth0(userId),
            cancellationToken: new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token
        );
    }


    private async Task<string> GetRole(string userId)
    {
        var roles = await managementApiClient.Users.GetRolesAsync(userId);
        if (roles.Any(x => x.Name == "Administrator"))
            return "Administrator";
        if (roles.Any(x => x.Name == "Inventory Manager"))
            return "Inventory Manager";
        return "User";
    }

    public async Task<UserDTO> BlockUser(string userId)
    {
        var user = await GetUser(userId);
        await managementApiClient.Users.UpdateAsync(userId, new UserUpdateRequest
        {
            Blocked = !user.IsBlocked
        });
        await _cache.RemoveAsync($"GET-USERINFO-ALL");
        await _cache.RemoveAsync($"GET-USERINFO-{userId}");
        return await GetUser(userId, true);
    }

    public async Task<bool> DeleteUser(string userId)
    {
        if (userId == _authContextProvider.User?.Identity?.Name)
            return false;

        await managementApiClient.Users.DeleteAsync(userId);
        await _cache.RemoveAsync($"GET-USERINFO-ALL");
        await _cache.RemoveAsync($"GET-USERINFO-{userId}");
        return true;
    }

    public async Task<UserDTO> AddUser(UserCreationDTO user)
    {
        var newUser = await managementApiClient.Users.CreateAsync(new UserCreateRequest
        {
            Connection = "Username-Password-Authentication",
            Email = user.Email,
            Password = user.Password,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = $"{user.FirstName} {user.LastName}",
            AppMetadata = new
            {
                user.Role
            }
        });
        await GetUsers(true);
        return await GetUser(newUser.UserId, true);
    }
}