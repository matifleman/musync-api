using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Musync.Domain;

namespace Musync.Persistance.Configurations
{
    internal sealed class BandConfiguration : IEntityTypeConfiguration<Band>
    {
        public void Configure(EntityTypeBuilder<Band> builder)
        {
            builder.HasMany(b => b.RequiredInstruments)
                .WithMany()
                .UsingEntity(
                    "BandInstrument",
                    r => r.HasOne(typeof(Instrument)).WithMany().HasForeignKey("InstrumentId"),
                    l => l.HasOne(typeof(Band)).WithMany().HasForeignKey("BandId"));
        }
    }
}