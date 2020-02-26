using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MindHorizon.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MindHorizon.Data.Mapping
{
    public class PostCategoryMapping : IEntityTypeConfiguration<PostCategory>
    {
        public void Configure(EntityTypeBuilder<PostCategory> builder)
        {
            builder.HasKey(t => new { t.CategoryId, t.PostId });
            builder
              .HasOne(p => p.Post)
              .WithMany(t => t.PostCategories)
              .HasForeignKey(f => f.PostId);

            builder
               .HasOne(p => p.Category)
               .WithMany(t => t.PostCategories)
               .HasForeignKey(f => f.CategoryId);
        }
    }

}
