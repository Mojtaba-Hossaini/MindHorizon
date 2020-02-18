using MindHorizon.ViewModels.Post;
using System.Collections.Generic;

namespace MindHorizon.ViewModels.Home
{
    public class HomePageViewModel
    {
        public HomePageViewModel(List<PostViewModel> post, List<PostViewModel> mostViewedPosts, List<PostViewModel> mostTalkPosts, List<PostViewModel> mostPopularPosts)
        {
            Post = post;
            MostViewedPosts = mostViewedPosts;
            MostTalkPosts = mostTalkPosts;
            MostPopularPosts = mostPopularPosts;
        }

        public List<PostViewModel> Post { get; set; }
        public List<PostViewModel> MostViewedPosts { get; }
        public List<PostViewModel> MostTalkPosts { get; }
        public List<PostViewModel> MostPopularPosts { get; }
    }
}
