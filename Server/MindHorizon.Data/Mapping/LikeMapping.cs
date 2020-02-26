using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MindHorizon.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MindHorizon.Data.Mapping
{
    public class LikeMapping : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.HasKey(t => new { t.PostId, t.IpAddress });
            builder
              .HasOne(p => p.Post)
              .WithMany(t => t.Likes)
              .HasForeignKey(f => f.PostId);
        }
    }
}
