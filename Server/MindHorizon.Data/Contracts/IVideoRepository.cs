using MindHorizon.ViewModels.Video;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindHorizon.Data.Contracts
{
    public interface IVideoRepository
    {
        string CheckVideoFileName(string fileName);
        Task<List<VideoViewModel>> GetPaginateVideosAsync(int offset, int limit, bool? titleSortAsc, bool? publishDateTimeSortAsc, string searchText);
    }
}
