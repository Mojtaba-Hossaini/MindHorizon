﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MindHorizon.Entities
{
    public class Category
    {
        [Key]
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }

        [ForeignKey("category")]
        public string ParentCategoryId { get; set; }
        public string Url { get; set; }

        public ICollection<PostCategory> PostCategories { get; set; }
        public virtual Category category { get; set; }
        public virtual List<Category> Categories { get; set; }
    }
}
