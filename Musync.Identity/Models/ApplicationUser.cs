using Microsoft.AspNetCore.Identity;

namespace Musync.Identity.Models
{
    public sealed class ApplicationUser : IdentityUser<int>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateOnly BornDate { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
