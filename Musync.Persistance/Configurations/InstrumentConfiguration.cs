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
                new Instrument { Id = 1, Name = "Guitar" },
                new Instrument { Id = 2, Name = "Piano" },
                new Instrument { Id = 3, Name = "Drums" },
                new Instrument { Id = 4, Name = "Bass" },
                new Instrument { Id = 5, Name = "Violin" },
                new Instrument { Id = 6, Name = "Saxophone" },
                new Instrument { Id = 7, Name = "Trumpet" },
                new Instrument { Id = 8, Name = "Flute" },
                new Instrument { Id = 9, Name = "Cello" },
                new Instrument { Id = 10, Name = "Synthesizer" }
            );
        }
    }
}
