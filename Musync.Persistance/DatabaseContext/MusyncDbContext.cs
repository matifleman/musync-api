using Microsoft.EntityFrameworkCore;
using Musync.Domain;

namespace Musync.Persistance.DatabaseContext
{
    public sealed class MusyncDbContext : DbContext
    {
        public MusyncDbContext(DbContextOptions<MusyncDbContext> options): base(options)
        { }

        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
