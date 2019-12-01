using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MindHorizon.Entities;

namespace MindHorizon.Data.Mapping
{
    public class VisitMapping : IEntityTypeConfiguration<Visit>
    {
        public void Configure(EntityTypeBuilder<Visit> builder)
        {
            builder.HasKey(t => new { t.PostId, t.IpAddress });
            builder
              .HasOne(p => p.Post)
              .WithMany(t => t.Visits)
              .HasForeignKey(f => f.PostId);
        }
    }
}
