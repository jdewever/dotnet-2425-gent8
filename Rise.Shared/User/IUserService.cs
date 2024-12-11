namespace Rise.Shared.User;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetUsers();

    Task<UserDto> GetUser(string userId);
    Task<UserDto> GetUserCache(string userId);
    Task<IEnumerable<UserDto>> GetUsersCached();
}