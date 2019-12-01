using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MindHorizon.Entities;

namespace MindHorizon.Data.Mapping
{
    public class BookmarkMapping : IEntityTypeConfiguration<Bookmark>
    {
        public void Configure(EntityTypeBuilder<Bookmark> builder)
        {
            builder.HasKey(t => new { t.UserId, t.PostId });
            builder
              .HasOne(p => p.Post)
              .WithMany(t => t.Bookmarks)
              .HasForeignKey(f => f.PostId);

            builder
               .HasOne(p => p.User)
               .WithMany(t => t.Bookmarks)
               .HasForeignKey(f => f.UserId);
        }
    }
}
