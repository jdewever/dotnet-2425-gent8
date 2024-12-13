namespace Rise.Shared.User;

public interface IUserService
{
    Task<UserDTO> GetUser(string userId, bool forceRefresh = false);
    Task<IEnumerable<UserDTO>> GetUsers(bool forceRefresh = false);
    Task<UserDTO> BlockUser(string userId);
    Task<bool> DeleteUser(string userId);
}