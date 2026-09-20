using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Musync.Domain;

namespace Musync.Persistance.Configurations
{
    internal sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Text)
                .IsRequired()
                .HasMaxLength(Comment.TextMaxLength);

            // Backs GetCommentsForPostAsync: filter on PostId, then page by descending Id.
            builder.HasIndex(c => new { c.PostId, c.Id });

            // No explicit OnDelete on either relation: both FKs are non-nullable, and EF's
            // default for a required relationship is Cascade - which is what "comments are
            // deleted with their post" needs. Same shape as PostLikeConfiguration.
            builder.HasOne(c => c.Post)
                .WithMany()
                .HasForeignKey(c => c.PostId);

            builder.HasOne(c => c.Author)
                .WithMany()
                .HasForeignKey(c => c.AuthorId);
        }
    }
}
