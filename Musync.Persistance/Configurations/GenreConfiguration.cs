using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Musync.Domain;
namespace Musync.Persistance.Configurations
{
    internal sealed class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasData(
                new Genre { Id = 1, Name = "Rock" },
                new Genre { Id = 2, Name = "Pop" },
                new Genre { Id = 3, Name = "Jazz" },
                new Genre { Id = 4, Name = "Classical" },
                new Genre { Id = 5, Name = "Hip Hop" },
                new Genre { Id = 6, Name = "Reggae" },
                new Genre { Id = 7, Name = "Electronic" },
                new Genre { Id = 8, Name = "Folk" },
                new Genre { Id = 9, Name = "Metal" },
                new Genre { Id = 10, Name = "Blues" }
            );
        }
    }
}
