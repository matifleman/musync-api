using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Musync.Domain;

namespace Musync.Persistance.DatabaseContext
{
    public sealed class MusyncDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public MusyncDbContext(DbContextOptions<MusyncDbContext> options): base(options)
        { }

        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MusyncDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
