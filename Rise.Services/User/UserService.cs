using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using Microsoft.Extensions.Caching.Hybrid;
using Rise.Shared.User;

namespace Rise.Services.User;

public class UserService : IUserService
{
    private readonly IManagementApiClient managementApiClient;
    private readonly HybridCache _cache;

    public UserService(IManagementApiClient managementApiClient, HybridCache cache)
    {
        this.managementApiClient = managementApiClient;
        _cache = cache;
    }

    public async Task<IEnumerable<UserDTO>> GetUsers()
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

    public async Task<IEnumerable<UserDTO>> GetUsersCached()
    {
        return await _cache.GetOrCreateAsync(
            $"GET-USERINFO-ALL", // Unique key for the cache
            async cancel => await GetUsers(),
            cancellationToken: new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token
        );
    }

    public async Task<UserDTO> GetUser(string userId)
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

    public async Task<UserDTO> GetUserCache(string userId)
    {
        return await _cache.GetOrCreateAsync(
            $"GET-USERINFO-{userId}", // Unique key for the cache
            async cancel => await GetUser(userId),
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
        return await GetUser(userId);
    }
}