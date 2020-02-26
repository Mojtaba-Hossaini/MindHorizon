using Microsoft.AspNetCore.Mvc;
using MindHorizon.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindHorizon.ViewComponents
{
    [ViewComponent(Name = "CategoryList")]
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
