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

        public async Task<IActionResult> Index(string duration, string typeOfPosts)
        {
            var isAjax = Request.Headers["X-Requested-with"] == "XMLHttpRequest";
            if (isAjax && typeOfPosts == "MostViewedPosts")
                return PartialView("_MostViewedPosts", await uw.PostRepository.MostViewedPosts(0, 3, duration));

            else if (isAjax && typeOfPosts == "MostTalkPosts")
                return PartialView("_MostTalkPosts", await uw.PostRepository.MostTalkPosts(0, 5, duration));
            else
            {
                var posts = uw.PostRepository.GetPaginatePost(0, 10, item => "", item => item.First().PersianPublishDate, "", null);
                var mostViewedPosts = await uw.PostRepository.MostViewedPosts(0, 3, "day");
                var mostTalkPosts = await uw.PostRepository.MostTalkPosts(0, 5,"day");
                var mostPopularPosts = await uw.PostRepository.MostPopularPosts(0, 5);
                var homePageViewModel = new HomePageViewModel(posts, mostViewedPosts, mostTalkPosts, mostPopularPosts);
                return View(homePageViewModel);
            }
            
        }
    }
}