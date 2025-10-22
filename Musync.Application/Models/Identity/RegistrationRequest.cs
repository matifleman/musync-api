using System.ComponentModel.DataAnnotations;

namespace Musync.Application.Models.Identity
{
    public record RegistrationRequest
    {
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        [MinLength(6)]
        public required string UserName { get; set; }
        public DateOnly BornDate { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
