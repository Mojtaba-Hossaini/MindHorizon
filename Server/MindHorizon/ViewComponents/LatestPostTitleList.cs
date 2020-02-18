using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MindHorizon.Data.Contracts;
using MindHorizon.ViewModels.Post;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MindHorizon.ViewComponents
{
    public class LatestPostTitleList : ViewComponent
    {
        private readonly IUnitOfWork _uw;
        public LatestPostTitleList(IUnitOfWork uw)
        {
            _uw = uw;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var postTitles = await _uw._Context.Post.Where(n => n.IsPublish == true && n.PublishDateTime <= DateTime.Now).OrderByDescending(n => n.PublishDateTime).Select(n => new PostViewModel { Title = n.Title, Url = n.Url, PostId = n.PostId }).Take(10).ToListAsync();
            return View(postTitles);
        }
    }
}
