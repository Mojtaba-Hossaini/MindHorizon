using MindHorizon.Entities;
using MindHorizon.ViewModels.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindHorizon.Data.Contracts
{
    public interface ICategoryRepository
    {
        Category FindByCategoryName(string categoryName);
        List<TreeViewCategory> GetAllCategories();
        Task<List<CategoryViewModel>> GetPaginateCategoriesAsync(int offset, int limit, bool? categoryNameSortAsc, bool? parentCategoryNameSortAsc, string searchText);
        bool IsExistCategory(string categoryName, string recentCategoryId = null);
    }
}
