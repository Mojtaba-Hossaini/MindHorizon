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
            var posts = uw.PostRepository.GetPaginatePost(0, 10, item => "", item => item.First().PersianPublishDate, "", null);
            var homePageViewModel = new HomePageViewModel(posts);
            return View(homePageViewModel);
        }
    }
}