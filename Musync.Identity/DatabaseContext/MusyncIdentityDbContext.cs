using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Musync.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Musync.Identity.DatabaseContext
{
    public sealed class MusyncIdentityDbContext(DbContextOptions<MusyncIdentityDbContext> options)
        : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
 }
