using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Musync.Domain;

namespace Musync.Persistance.Configurations
{
    internal sealed class ReleaseConfiguration : IEntityTypeConfiguration<Release>
    {
        public void Configure(EntityTypeBuilder<Release> builder)
        {
            builder.HasOne(r => r.Band)
                .WithMany()
                .HasForeignKey(r => r.BandId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
