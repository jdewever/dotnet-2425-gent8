using System.ComponentModel.DataAnnotations;

namespace Rise.Domain.DomainClasses
{
    public class UserTransaction : Entity
    {
        [Required]
        public required int UserId { get; init; }

        [Required]
        public required string Type { get; init; }
    }
}
