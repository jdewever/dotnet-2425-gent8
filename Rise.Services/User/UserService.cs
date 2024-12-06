using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using Rise.Shared.User;

namespace Rise.Services.User;

public class UserService : IUserService
{
    private readonly IManagementApiClient managementApiClient;

    public UserService(IManagementApiClient managementApiClient)
    {
        this.managementApiClient = managementApiClient;
    }

    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        var rawUsers = await managementApiClient.Users.GetAllAsync(new GetUsersRequest(), new PaginationInfo());
        return await Task.WhenAll(rawUsers.Select(async user => new UserDto
        {
            Email = user.Email,
            IsBlocked = user.Blocked ?? false,
            FullName = user.FullName,
            Picture = user.Picture,
            Role = await GetRole(user.UserId)
        }));
    }

    public async Task<UserDto> GetUser(string userId)
    {
        var user = await managementApiClient.Users.GetAsync(userId);
        return new UserDto
        {
            Email = user.Email,
            IsBlocked = user.Blocked ?? false,
            FullName = user.FullName,
            Picture = user.Picture,
            Role = await GetRole(user.UserId)
        };
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
}