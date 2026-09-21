using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Musync.Domain;

namespace Musync.Application.Features.User
{
    // Everything CurrentUserDTO reads - follower/followed counts and favourite tags - in one place.
    // The app replaces its session user with whatever an auth or "update me" endpoint returns, so a
    // response mapped from a partly-loaded user silently wipes the parts that weren't loaded (a
    // genres update used to come back with no instruments and zero followers).
    public static class SelfProfileQueryExtensions
    {
        public static IQueryable<ApplicationUser> WithSelfProfile(this IQueryable<ApplicationUser> users)
        {
            return users
                .Include(u => u.Followers)
                .Include(u => u.Followed)
                .Include(u => u.FavoriteInstruments)
                .Include(u => u.FavoriteGenres);
        }

        // A fresh, untracked read after a save, so the response reflects what was actually persisted
        // rather than whichever navigations the handler happened to have loaded.
        public static Task<ApplicationUser> LoadSelfProfileAsync(
            this UserManager<ApplicationUser> userManager, int userId, CancellationToken cancellationToken = default)
        {
            return userManager.Users
                .AsNoTracking()
                .WithSelfProfile()
                .FirstAsync(u => u.Id == userId, cancellationToken);
        }
    }
}
