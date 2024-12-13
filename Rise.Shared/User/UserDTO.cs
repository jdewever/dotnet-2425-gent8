using System.ComponentModel.DataAnnotations;
namespace Rise.Shared.User;

public class UserDTO
{
    public required string UserID { get; set; }
    public required string Email { get; set; }
    public required bool IsBlocked { get; set; }
    public required string Picture { get; set; }
    public required string FullName { get; set; }
    public required string Role { get; set; }
}

public class UserCreationDTO
{
    [Required(ErrorMessage = "Email is verplicht.")]
    [EmailAddress(ErrorMessage = "Email is niet geldig.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Wachtwoord is verplicht.")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Voornaam is verplicht.")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Familienaam is verplicht.")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Rol is verplicht.")]
    public required string Role { get; set; }
}