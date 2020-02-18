using Microsoft.AspNetCore.Mvc;
using MindHorizon.Data.Contracts;
using System.Threading.Tasks;

namespace MindHorizon.ViewComponents
{
    public class CategoryList : ViewComponent
    {
        private readonly IUnitOfWork _uw;
        public CategoryList(IUnitOfWork uw)
        {
            _uw = uw;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _uw.CategoryRepository.GetAllCategoriesAsync());
        }
    }
}
