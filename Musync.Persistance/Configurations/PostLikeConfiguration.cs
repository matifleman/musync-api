using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Musync.Domain;

namespace Musync.Persistance.Configurations
{
    internal sealed class PostLikeConfiguration : IEntityTypeConfiguration<PostLike>
    {
        public void Configure(EntityTypeBuilder<PostLike> builder)
        {
            // Unique so a double-tap race can't create two likes for the same user+post;
            // LikePostCommandHandler's check-then-insert is not atomic on its own.
            builder.HasIndex(pl => new { pl.UserId, pl.PostId })
                .IsUnique();

            builder.HasOne(pl => pl.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(p => p.PostId);

            builder.HasOne(pl => pl.User)
                .WithMany()
                .HasForeignKey(pl => pl.UserId);
        }
    }
}
