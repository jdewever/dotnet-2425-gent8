namespace Rise.Shared.User
{
    public class UserDto
    {
        public required string Email { get; set; }
        public required string FullName { get; set; }
        public required string Picture { get; set; }
        public required bool IsBlocked { get; set; }
    }
}