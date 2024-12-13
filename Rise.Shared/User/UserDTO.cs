namespace Rise.Shared.User
{
    public class UserDTO
    {
        public required string UserID { get; set; }
        public required string Email { get; set; }
        public required bool IsBlocked { get; set; }
        public required string Picture { get; set; }
        public required string FullName { get; set; }
        public required string Role { get; set; }
    }
}