using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindHorizon.Common;
using MindHorizon.Data.Contracts;
using MindHorizon.Entities;
using MindHorizon.ViewModels.Home;

namespace MindHorizon.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _uw;
        private readonly IHttpContextAccessor _accessor;
        public HomeController(IUnitOfWork uw, IHttpContextAccessor accessor)
        {
            _uw = uw;
            _accessor = accessor;
        }

        public async Task<IActionResult> Index(string duration,string TypeOfPosts)
        {
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax && TypeOfPosts == "MostViewedPosts")
                return PartialView("_MostViewedPosts", await _uw.PostRepository.MostViewedPostsAsync(0, 3, duration));


            else if (isAjax && TypeOfPosts == "MostTalkPosts")
                return PartialView("_MostTalkPosts", await _uw.PostRepository.MostTalkPosts(0, 5, duration));

            else
            {
                int countPostsPublished = _uw.PostRepository.CountPostsPublished();
                var post = await _uw.PostRepository.GetPaginatePostsAsync(0, 10, "PublishDateTime desc", "", true);
                var mostViewedPosts = await _uw.PostRepository.MostViewedPostsAsync(0, 3, "day");
                var mostTalkPosts = await _uw.PostRepository.MostTalkPosts(0, 5, "day");
                var mostPopulerPosts = await _uw.PostRepository.MostPopularPosts(0, 5);
                var videos = await _uw.VideoRepository.GetPaginateVideosAsync(0, 10, "PublishDateTime desc", "");
                var homePageViewModel = new HomePageViewModel(post, mostViewedPosts,mostTalkPosts,mostPopulerPosts, videos, countPostsPublished);
                return View(homePageViewModel);
            }
           
        }

        [Route("Posts/{postId}/{url}")]
        public async Task<IActionResult> PostDetails(string postId, string url)
        {
            string ipAddress = _accessor.HttpContext?.Connection?.RemoteIpAddress.ToString();
            int userId = User.Identity.GetUserId<int>();
            Visit visit = _uw.BaseRepository<Visit>().FindByConditionAsync(n => n.PostId == postId && n.IpAddress == ipAddress).Result.FirstOrDefault();
            if (visit != null && visit.LastVisitDateTime.Date != DateTime.Now.Date)
            {
                visit.NumberOfVisit = visit.NumberOfVisit + 1;
                visit.LastVisitDateTime = DateTime.Now;
                await _uw.Commit();
            }
            else if (visit == null)
            {
                visit = new Visit { IpAddress = ipAddress, LastVisitDateTime = DateTime.Now, PostId = postId, NumberOfVisit = 1 };
                await _uw.BaseRepository<Visit>().CreateAsync(visit);
                await _uw.Commit();
            }

            var post = await _uw.PostRepository.GetPostByIdAsync(postId, userId);
            var postComments = await _uw.PostRepository.GetPostCommentsAsync(postId);
            var nextAndPreviousPost = await _uw.PostRepository.GetNextAndPreviousPost(post.PublishDateTime);
            var postRelated = await _uw.PostRepository.GetRelatedPostsAsync(2, post.TagIdsList, postId);
            var postDetailsViewModel = new PostDetailsViewModel(post, postComments, postRelated, nextAndPreviousPost);
            return View(postDetailsViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> GetPostsPaginate(int limit, int offset)
        {
            int countPostsPublished = _uw.PostRepository.CountPostsPublished();
            var posts = await _uw.PostRepository.GetPaginatePostsAsync(offset, limit, "PublishDateTime desc", "", true);
            return PartialView("_PostsPaginate", new PostPaginateViewModel(countPostsPublished, posts));
        }


        [Route("Category/{categoryId}/{url}")]
        public async Task<IActionResult> PostsInCategory(string categoryId, string url)
        {
            if (!categoryId.HasValue())
                return NotFound();
            else
            {
                var category = await _uw.BaseRepository<Category>().FindByIdAsync(categoryId);
                if (category == null)
                    return NotFound();
                else
                    return View("PostsInCategoryAndTag", new CategoryOrTagInfoViewModel { Id = category.CategoryId, Title = category.CategoryName, IsCategory = true });




            }
        }

        [HttpGet]
        public async Task<ActionResult> GetPostsInCategoryAndTag(int pageIndex, int pageSize, string categoryId, string tagId)
        {
            if (categoryId.HasValue())
                return Json(await _uw.PostRepository.GetPostsInCategoryAsync(categoryId, pageIndex, pageSize));

            else
                return Json(await _uw.PostRepository.GetPostsInCategoryAsync(tagId, pageIndex, pageSize));
        }

        [Route("Tag/{tagId}")]
        public async Task<IActionResult> PostsInTag(string tagId)
        {
            if (!tagId.HasValue())
                return NotFound();
            else
            {
                var tag = await _uw.BaseRepository<Tag>().FindByIdAsync(tagId);
                if (tag == null)
                    return NotFound();
                else
                    return View("PostsInCategoryAndTag", new CategoryOrTagInfoViewModel { Id = tag.TagId, Title = tag.TagName, IsCategory = false });

            }
        }

        [Route("Videos")]
        public async Task<IActionResult> Videos()
        {
            return View(await _uw.BaseRepository<Video>().FindAllAsync());
        }

        [Route("Video/{videoId}")]
        public async Task<IActionResult> VideoDetails(string videoId)
        {
            if (!videoId.HasValue())
                return NotFound();
            else
            {
                var video = await _uw.BaseRepository<Video>().FindByIdAsync(videoId);
                if (video == null)
                    return NotFound();
                else
                    return View(video);
            }
        }

        [HttpGet]
        public async Task<JsonResult> LikeOrDisLike(string postId, bool isLike)
        {
            string ipAddress = _accessor.HttpContext?.Connection?.RemoteIpAddress.ToString();
            Like likeOrDislike = _uw.BaseRepository<Like>().FindByConditionAsync(l => l.PostId == postId && l.IpAddress == ipAddress).Result.FirstOrDefault();
            if (likeOrDislike == null)
            {
                likeOrDislike = new Like { PostId = postId, IpAddress = ipAddress, IsLiked = isLike };
                await _uw.BaseRepository<Like>().CreateAsync(likeOrDislike);
            }
            else
                likeOrDislike.IsLiked = isLike;

            await _uw.Commit();
            var likeAndDislike = _uw.PostRepository.NumberOfLikeAndDislike(postId);
            return Json(new { like = likeAndDislike.NumberOfLike, dislike = likeAndDislike.NumberOfDisLike });
        }


        [HttpGet]
        public async Task<IActionResult> BookmarkPost(string postId)
        {
            if (User.Identity.IsAuthenticated)
            {
                int userId = User.Identity.GetUserId<int>();
                Bookmark bookmark = _uw.BaseRepository<Bookmark>().FindByConditionAsync(l => l.PostId == postId && l.UserId == userId).Result.FirstOrDefault();
                if (bookmark == null)
                {
                    bookmark = new Bookmark { PostId = postId, UserId = userId };
                    await _uw.BaseRepository<Bookmark>().CreateAsync(bookmark);
                    await _uw.Commit();
                    return Json(true);
                }
                else
                {
                    _uw.BaseRepository<Bookmark>().Delete(bookmark);
                    await _uw.Commit();
                    return Json(false);
                }
            }

            else
                return PartialView("_SignIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(string searchText) => View(await _uw.PostRepository.SearchInPosts(searchText));

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Error404()
        {
            return View();
        }

    }
}