using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Musync.Domain;

namespace Musync.Persistance.Configurations
{
    internal sealed class InstrumentConfiguration : IEntityTypeConfiguration<Instrument>
    {
        public void Configure(EntityTypeBuilder<Instrument> builder)
        {
            builder.HasData(
                new Instrument { Id = 1, Name = "Guitar", Image = "instruments/guitar.svg" },
                new Instrument { Id = 2, Name = "Piano", Image = "instruments/piano.svg" },
                new Instrument { Id = 3, Name = "Drums", Image = "instruments/drums.svg" },
                new Instrument { Id = 4, Name = "Bass", Image = "instruments/bass.svg" },
                new Instrument { Id = 5, Name = "Violin", Image = "instruments/violin.svg" },
                new Instrument { Id = 6, Name = "Saxophone", Image = "instruments/saxophone.svg" },
                new Instrument { Id = 7, Name = "Trumpet", Image = "instruments/trumpet.svg" },
                new Instrument { Id = 8, Name = "Flute", Image = "instruments/flute.svg" },
                new Instrument { Id = 9, Name = "Cello", Image = "instruments/cello.svg" },
                new Instrument { Id = 10, Name = "Synthesizer", Image = "instruments/synthesizer.svg" }
            );
        }
    }
}
