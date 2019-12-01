using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MindHorizon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindHorizon.Data.Mapping
{
    public class PostTagMapping : IEntityTypeConfiguration<PostTag>
    {
        public void Configure(EntityTypeBuilder<PostTag> builder)
        {
            builder.HasKey(t => new { t.TagId, t.PostId });
            builder
              .HasOne(p => p.Post)
              .WithMany(t => t.PostTags)
              .HasForeignKey(f => f.PostId);

            builder
               .HasOne(p => p.Tag)
               .WithMany(t => t.PostTags)
               .HasForeignKey(f => f.TagId);
        }
    }
}
