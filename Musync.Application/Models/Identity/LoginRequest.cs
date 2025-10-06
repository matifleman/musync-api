using System.ComponentModel.DataAnnotations;

namespace Musync.Application.Models.Identity
{
    public record LoginRequest 
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    };
}
