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
        int CountPostsPublishedOrDraft(bool? isPublish);
        int CountPostsPublished();
        List<PostViewModel> GetPaginatePosts(int offset, int limit, Func<IGrouping<string, PostViewModel>, Object> orderByAscFunc, Func<IGrouping<string, PostViewModel>, Object> orderByDescFunc, string searchText, bool? isPublish);
        Task<List<PostViewModel>> MostViewedPosts(int offset, int limit, string duration);
        Task<List<PostViewModel>> MostTalkPosts(int offset, int limit, string duration);
        Task<List<PostViewModel>> MostPopularPosts(int offset, int limit);
        Task<PostViewModel> GetPostById(string postId, int userId);
        Task<List<Comment>> GetPostCommentsAsync(string postId);
        Task BindSubComments(Comment comment);
        Task<List<PostViewModel>> GetNextAndPreviousPost(DateTime? PublishDateTime);
        Task<List<PostViewModel>> GetRelatedPosts(int number, List<string> tagIdList, string postId);
        Task<List<PostsInCategoriesAndTagsViewModel>> GetPostsInCategoryAndTag(string categoryId, string TagId, int pageIndex, int pageSize)
        Task<List<PostViewModel>> GetUserBookmarksAsync(int userId);
        Task<List<PostViewModel>> SearchInPosts(string textSearch);
        PostViewModel NumberOfLikeAndDislike(string postId);

    }
}
