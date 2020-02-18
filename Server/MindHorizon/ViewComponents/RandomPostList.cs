using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MindHorizon.Common;
using MindHorizon.Data.Contracts;
using MindHorizon.ViewModels.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindHorizon.ViewComponents
{
    public class RandomPostList : ViewComponent
    {
        private readonly IUnitOfWork _uw;
        public RandomPostList(IUnitOfWork uw)
        {
            _uw = uw;
        }

        public async Task<IViewComponentResult> InvokeAsync(int number)
        {
            var postsList = new List<PostViewModel>();
            int randomRow;
            for (int i=0;i<number;i++)
            {
                randomRow = CustomMethods.RandomNumber(1, _uw.PostRepository.CountPostPublished()+1);
                var posts = await _uw._Context.Post.Where(n => n.IsPublish == true && n.PublishDateTime <= DateTime.Now).Select(n => new PostViewModel { Title = n.Title, Url = n.Url, PostId = n.PostId, ImageName = n.ImageName }).Skip(randomRow-1).Take(1).FirstOrDefaultAsync();
                postsList.Add(posts);
            }
           
            return View(postsList);
        }
    }
}
