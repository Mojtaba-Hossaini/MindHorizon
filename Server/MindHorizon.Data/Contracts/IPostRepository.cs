using MindHorizon.ViewModels.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindHorizon.Data.Contracts
{
    public interface IPostRepository
    {
        string CheckPostFileName(string fileName);
        List<PostViewModel> GetPaginatePost(int offset, int limit, Func<IGrouping<string, PostViewModel>, object> orderByAscFunc, Func<IGrouping<string, PostViewModel>, object> orderByDescFunc, string searchText, bool? isPublish);
        Task<List<PostViewModel>> MostViewedPosts(int offset, int limit, string duration);
        Task<List<PostViewModel>> MostTalkPosts(int offset, int limit, string duration);
        Task<List<PostViewModel>> MostPopularPosts(int offset, int limit);
        int CountPostPublished();
    }
}
