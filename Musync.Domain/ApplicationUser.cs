using Microsoft.AspNetCore.Identity;

namespace Musync.Domain
{
    public sealed class ApplicationUser : IdentityUser<int>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateOnly BornDate { get; set; }
        public required string ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public override required string Email { get; set; }
        public override required string UserName { get; set; }
        public ICollection<Instrument>? FavoriteInstruments { get; set; } = [];
        public ICollection<Genre>? FavoriteGenres { get; set; } = [];
        public ICollection<ApplicationUser>? Followers { get; set; } = [];
        public ICollection<ApplicationUser>? Followed { get; set; } = [];
        // Deliberately no default in the model: the column defaults to false, which is also
        // the CLR default. A model default of true would make EF treat an explicit false as
        // "unset" and let the database write true for a brand-new user.
        public bool OnboardingCompleted { get; set; }

        public bool IsFollowing(int otherUserId)
        {
            if (Followed is null || Followed.Count == 0) return false;
            return Followed.Any(u => u.Id == otherUserId);
        }
    }
}
