using MindHorizon.Entities;
using MindHorizon.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MindHorizon.Data.Contracts
{
    public interface ICategoryRepository
    {
        Category FindByCategoryName(string categoryName);
        Task<List<TreeViewCategory>> GetAllCategoriesAsync();
        bool IsExistCategory(string categoryName, string recentCategoryId = null);
        Task<List<CategoryViewModel>> GetPaginateCategoriesAsync(int offset, int limit, string orderBy, string searchText);
    }
}
