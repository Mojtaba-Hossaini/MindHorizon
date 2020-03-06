using MindHorizon.Entities;
using MindHorizon.ViewModels.Home;
using MindHorizon.ViewModels.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MindHorizon.Data.Contracts
{
    public interface IPostRepository
    {
        string CheckPostFileName(string fileName);
        int CountPosts();
        int CountFuturePublishedPosts();
        public int CountPostsPublishedOrDraft(bool isPublish);
        int CountPostsPublished();
        Task<List<PostViewModel>> GetPaginatePostsAsync(int offset, int limit, string orderBy, string searchText, bool? isPublish);
        Task<List<PostViewModel>> MostViewedPostsAsync(int offset, int limit, string duration);
        Task<List<PostViewModel>> MostTalkPosts(int offset, int limit, string duration);
        Task<List<PostViewModel>> MostPopularPosts(int offset, int limit);
        Task<PostViewModel> GetPostByIdAsync(string postId, int userId);
        Task<List<Comment>> GetPostCommentsAsync(string postId);
        Task BindSubComments(Comment comment);
        Task<List<PostViewModel>> GetNextAndPreviousPost(DateTime? PublishDateTime);
        Task<List<PostViewModel>> GetRelatedPostsAsync(int number, List<string> tagIdList, string postId);
        Task<List<PostsInCategoriesAndTagsViewModel>> GetPostsInCategoryAsync(string categoryId, int pageIndex, int pageSize);
        Task<List<PostsInCategoriesAndTagsViewModel>> GetPostsInTagAsync(string TagId, int pageIndex, int pageSize);
        Task<List<PostViewModel>> GetUserBookmarksAsync(int userId);
        Task<List<PostViewModel>> SearchInPosts(string textSearch);
        PostViewModel NumberOfLikeAndDislike(string postId);

    }
}
