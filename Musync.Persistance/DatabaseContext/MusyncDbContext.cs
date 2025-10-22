using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Musync.Domain;
using Musync.Domain.Common;

namespace Musync.Persistance.DatabaseContext
{
    public sealed class MusyncDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public MusyncDbContext(DbContextOptions<MusyncDbContext> options): base(options)
        { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Band> Bands { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Instrument> Instruments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MusyncDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach(var entry in ChangeTracker.Entries<BaseEntity>().Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
                if(entry.State == EntityState.Added)
                    entry.Entity.CreatedAt = DateTime.UtcNow;
            }
                 
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
