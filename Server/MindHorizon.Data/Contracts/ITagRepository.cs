using MindHorizon.Entities;
using MindHorizon.ViewModels.Tag;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindHorizon.Data.Contracts
{
    public interface ITagRepository
    {
        Task<List<TagViewModel>> GetPaginateTagsAsync(int offset, int limit, bool? tagNameSortAsc, string searchText);
        bool IsExistTag(string tagName, string recentTagId = null);
        Task<List<PostTag>> InsertPostTags(string[] tags, string postId = null);
    }
}
