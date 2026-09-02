using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Musync.Domain;

namespace Musync.Persistance.Configurations
{
    internal sealed class BandFollowerConfiguration : IEntityTypeConfiguration<BandFollower>
    {
        public void Configure(EntityTypeBuilder<BandFollower> builder)
        {
            builder.HasIndex(bf => new { bf.BandId, bf.UserId }).IsUnique();

            builder.HasOne(bf => bf.Band)
                .WithMany()
                .HasForeignKey(bf => bf.BandId);

            builder.HasOne(bf => bf.User)
                .WithMany()
                .HasForeignKey(bf => bf.UserId);
        }
    }
}
