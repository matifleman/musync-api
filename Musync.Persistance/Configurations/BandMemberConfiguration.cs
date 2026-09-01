using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Musync.Domain;

namespace Musync.Persistance.Configurations
{
    internal sealed class BandMemberConfiguration : IEntityTypeConfiguration<BandMember>
    {
        public void Configure(EntityTypeBuilder<BandMember> builder)
        {
            builder.HasIndex(bm => new { bm.BandId, bm.InstrumentId }).IsUnique();
            builder.HasIndex(bm => new { bm.BandId, bm.UserId }).IsUnique();

            builder.HasOne(bm => bm.Band)
                .WithMany(b => b.Members)
                .HasForeignKey(bm => bm.BandId);

            builder.HasOne(bm => bm.User)
                .WithMany()
                .HasForeignKey(bm => bm.UserId);

            builder.HasOne(bm => bm.Instrument)
                .WithMany()
                .HasForeignKey(bm => bm.InstrumentId);
        }
    }
}