using MindHorizon.ViewModels.Post;
using MindHorizon.ViewModels.Video;
using System.Collections.Generic;

namespace MindHorizon.ViewModels.Home
{
    public class HomePageViewModel
    {
        public HomePageViewModel(List<PostViewModel> post, List<PostViewModel> mostViewedPosts, List<PostViewModel> mostTalkPosts, List<PostViewModel> mostPopularPosts, List<VideoViewModel> videos, int countPostsPublished)
        {
            Post = post;
            MostViewedPosts = mostViewedPosts;
            MostTalkPosts = mostTalkPosts;
            MostPopularPosts = mostPopularPosts;
            Videos = videos;
            CountPostsPublished = countPostsPublished;
        }

        public List<PostViewModel> Post { get; set; }
        public List<PostViewModel> MostViewedPosts { get; }
        public List<PostViewModel> MostTalkPosts { get; }
        public List<PostViewModel> MostPopularPosts { get; }
        public List<VideoViewModel> Videos { get; }
        public int CountPostsPublished { get; set; }
    }
}
