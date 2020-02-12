using MindHorizon.ViewModels.Category;
using System.Collections.Generic;

namespace MindHorizon.ViewModels.Post
{
    public class PostCategoriesViewModel
    {
        public PostCategoriesViewModel(List<TreeViewCategory> categories, string[] categoryId)
        {
            Categories = categories;
            CategoryId = categoryId;
        }

        public List<TreeViewCategory> Categories { get; set; }
        public string[] CategoryId { get; set; }
    }
}
