using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Musync.Domain;

namespace Musync.Persistance.Configurations
{
    internal sealed class SongConfiguration : IEntityTypeConfiguration<Song>
    {
        public void Configure(EntityTypeBuilder<Song> builder)
        {
            builder.HasOne(s => s.Release)
                .WithMany(r => r.Songs)
                .HasForeignKey(s => s.ReleaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
