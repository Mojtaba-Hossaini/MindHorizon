using MindHorizon.ViewModels.Video;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MindHorizon.Data.Contracts
{
    public interface IVideoRepository
    {
        string CheckVideoFileName(string fileName);
        List<VideoViewModel> GetPaginateVideos(int offset, int limit, Func<VideoViewModel, Object> orderByAscFunc, Func<VideoViewModel, Object> orderByDescFunc, string searchText);
    }
}
