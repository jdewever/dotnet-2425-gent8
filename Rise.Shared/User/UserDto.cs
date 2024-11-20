namespace Rise.Shared.User
{
    public class UserDto
    {
        public required string Email { get; set; }
        
        // zit niet in auth0
        // public required string FirstName { get; set; }
        // public required string LastName { get; set; }
        public required bool IsBlocked { get; set; }
        public required string Picture { get; set; }
        public required string Fullname { get; set; }
    }
}