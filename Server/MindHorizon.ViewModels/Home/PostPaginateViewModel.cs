using MindHorizon.ViewModels.Post;
using System;
using System.Collections.Generic;
using System.Text;

namespace MindHorizon.ViewModels.Home
{
    public class PostPaginateViewModel
    {
        public PostPaginateViewModel(int postsCount, List<PostViewModel> posts)
        {
            PostsCount = postsCount;
            Posts = posts;
        }

        public int PostsCount { get; set; }
        public List<PostViewModel> Posts { get; set; }
    }

}
