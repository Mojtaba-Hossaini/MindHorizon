using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MindHorizon.Data.Mapping
{
    public static class MindHorizonMapping
    {
        public static void AddCustomMindHorizonMappings(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookmarkMapping());
            modelBuilder.ApplyConfiguration(new LikeMapping());
            modelBuilder.ApplyConfiguration(new PostCategoryMapping());
            modelBuilder.ApplyConfiguration(new PostTagMapping());
            modelBuilder.ApplyConfiguration(new VisitMapping());
        }

    }
}
