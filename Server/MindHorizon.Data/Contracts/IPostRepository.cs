using MindHorizon.Entities;
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
        int CountPostsPublished();
        List<PostViewModel> GetPaginatePosts(int offset, int limit, Func<IGrouping<string, PostViewModel>, Object> orderByAscFunc, Func<IGrouping<string, PostViewModel>, Object> orderByDescFunc, string searchText, bool? isPublish);
        Task<List<PostViewModel>> MostViewedPosts(int offset, int limit, string duration);
        Task<List<PostViewModel>> MostTalkPosts(int offset, int limit, string duration);
        Task<List<PostViewModel>> MostPopularPosts(int offset, int limit);
        Task<PostViewModel> GetPostById(string postId);
        Task<List<Comment>> GetPostCommentsAsync(string postId);
        Task BindSubComments(Comment comment);
        Task<List<PostViewModel>> GetNextAndPreviousPost(DateTime? PublishDateTime);
        Task<List<PostViewModel>> GetRelatedPosts(int number, List<string> tagIdList, string postId);
        Task<List<PostViewModel>> GetPostsInCategoryAndTag(string categoryId, string TagId);

        Task<List<PostViewModel>> GetUserBookmarksAsync(int userId);
    }
}
