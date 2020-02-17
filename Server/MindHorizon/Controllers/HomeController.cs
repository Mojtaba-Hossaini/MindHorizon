using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MindHorizon.Data.Contracts;
using MindHorizon.ViewModels.Home;

namespace MindHorizon.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork uw;

        public HomeController(IUnitOfWork uw)
        {
            this.uw = uw;
        }

        public async Task<IActionResult> Index()
        {
            var post = await uw.PostRepository.GetPaginatePostAsync(0, 10, null, null, null, null, false, "", true);
            var homePageViewModel = new HomePageViewModel(post);
            return View(homePageViewModel);
        }
    }
}