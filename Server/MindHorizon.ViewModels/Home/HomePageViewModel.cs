using MindHorizon.ViewModels.Post;
using System.Collections.Generic;

namespace MindHorizon.ViewModels.Home
{
    public class HomePageViewModel
    {
        public HomePageViewModel(List<PostViewModel> post)
        {
            Post = post;
        }

        public List<PostViewModel> Post { get; set; }

    }
}
