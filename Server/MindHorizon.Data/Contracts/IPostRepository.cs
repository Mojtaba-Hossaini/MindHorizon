using MindHorizon.ViewModels.Post;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindHorizon.Data.Contracts
{
    public interface IPostRepository
    {
        string CheckPostFileName(string fileName);
        Task<List<PostViewModel>> GetPaginatePostAsync(int offset, int limit, bool? titleSortAsc, bool? visitSortAsc, bool? likeSortAsc, bool? dislikeSortAsc, bool? publishDateTimeSortAsc, string searchText);
    }
}
