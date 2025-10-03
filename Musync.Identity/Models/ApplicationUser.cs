using Microsoft.AspNetCore.Identity;

namespace Musync.Identity.Models
{
    public sealed class ApplicationUser : IdentityUser<int>
    {
        public required string FirstName;
        public required string LastName;
        public DateOnly BornDate;
        public required string ProfilePicture;
    }
}
