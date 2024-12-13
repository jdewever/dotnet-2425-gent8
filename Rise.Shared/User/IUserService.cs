namespace Rise.Shared.User;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetUsers();
    Task<UserDTO> GetUser(string userId);
    Task<UserDTO> GetUserCache(string userId);
    Task<IEnumerable<UserDTO>> GetUsersCached();
}