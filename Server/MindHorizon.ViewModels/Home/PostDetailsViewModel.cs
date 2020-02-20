using MindHorizon.Entities;
using MindHorizon.ViewModels.Post;
using System.Collections.Generic;

namespace MindHorizon.ViewModels.Home
{
    public class PostDetailsViewModel
    {
        public PostDetailsViewModel(PostViewModel post, List<Comment> comments, List<PostViewModel> postRelated, List<PostViewModel> nextAndPreviousPost)
        {
            Post = post;
            Comments = comments;
            PostRelated = postRelated;
            NextAndPreviousPost = nextAndPreviousPost;
        }
        public PostViewModel Post { get; set; }
        public List<Comment> Comments { get; set; }
        public List<PostViewModel> PostRelated { get; set; }
        public List<PostViewModel> NextAndPreviousPost { get; set; }
    }
}
