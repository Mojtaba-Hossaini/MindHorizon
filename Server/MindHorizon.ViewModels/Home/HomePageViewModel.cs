using MindHorizon.ViewModels.Post;
using MindHorizon.ViewModels.Video;
using System;
using System.Collections.Generic;
using System.Text;

namespace MindHorizon.ViewModels.Home
{
    public  class HomePageViewModel
    {
        public HomePageViewModel(List<PostViewModel> posts, List<PostViewModel> mostViewedPosts, List<PostViewModel> mostTalkPosts, List<PostViewModel> mostPopularPosts, List<VideoViewModel> videos, int countPostsPublished)
        {
            Posts = posts;
            MostViewedPosts = mostViewedPosts;
            MostTalkPosts = mostTalkPosts;
            MostPopularPosts = mostPopularPosts;
            Videos = videos;
            CountPostsPublished = countPostsPublished;
        }

        public List<PostViewModel> Posts { get; set; }
        public List<PostViewModel> MostViewedPosts { get; set; }
        public List<PostViewModel> MostTalkPosts { get; set; }
        public List<PostViewModel> MostPopularPosts { get; set; }
        public List<VideoViewModel> Videos { get; set; }
        public int CountPostsPublished { get; set; }
    }
}
