using MindHorizon.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace MindHorizon.ViewModels.Post
{
    public class PostCategoriesViewModel
    {
        public PostCategoriesViewModel(List<TreeViewCategory> categories, string[] categoryIds)
        {
            Categories = categories;
            CategoryIds = categoryIds;
        }

        public List<TreeViewCategory> Categories { get; set; }
        public string[] CategoryIds { get; set; }
    }
}
