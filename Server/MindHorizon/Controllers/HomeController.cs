using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindHorizon.Data.Contracts;
using MindHorizon.Entities;
using MindHorizon.ViewModels.Home;

namespace MindHorizon.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork uw;
        private readonly IHttpContextAccessor accessor;

        public HomeController(IUnitOfWork uw, IHttpContextAccessor accessor)
        {
            this.uw = uw;
            this.accessor = accessor;
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
                var videos = await uw.VideoRepository.GetPaginateVideosAsync(0, 10, null, false, "");
                var homePageViewModel = new HomePageViewModel(posts, mostViewedPosts, mostTalkPosts, mostPopularPosts, videos);
                return View(homePageViewModel);
            }
            
        }

        [Route("Posts/{postId}/{url}")]
        public async Task<IActionResult> PostDetails(string postId, string url)
        {
            string ipAddress = accessor.HttpContext?.Connection?.RemoteIpAddress.ToString();
            Visit visit = uw.BaseRepository<Visit>().FindByConditionAsync(n => n.PostId == postId && n.IpAddress == ipAddress).Result.FirstOrDefault();
            if (visit != null && visit.LastVisitDateTime.Date != DateTime.Now.Date)
            {
                visit.NumberOfVisit = visit.NumberOfVisit + 1;
                visit.LastVisitDateTime = DateTime.Now;
                await uw.Commit();
            }
            else if (visit == null)
            {
                visit = new Visit { IpAddress = ipAddress, LastVisitDateTime = DateTime.Now, PostId = postId, NumberOfVisit = 1 };
                await uw.BaseRepository<Visit>().CreateAsync(visit);
                await uw.Commit();
            }

            var post = await uw.PostRepository.GetPostById(postId);
            var postComments = await uw.PostRepository.GetPostCommentsAsync(postId);
            var nextAndPreviousPost = await uw.PostRepository.GetNextAndPreviousPost(post.PublishDateTime);
            var postRelated = await uw.PostRepository.GetRelatedPost(2, post.TagIdsList, postId);
            var postDetailsViewModel = new PostDetailsViewModel(post, postComments, postRelated, nextAndPreviousPost);
            return View(postDetailsViewModel);
        }
    }
}