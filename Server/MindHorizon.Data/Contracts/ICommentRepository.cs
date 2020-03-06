using MindHorizon.ViewModels.Comments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindHorizon.Data.Contracts
{
    public interface ICommentRepository
    {
        int CountUnAnsweredComments();
        Task<List<CommentViewModel>> GetPaginateCommentsAsync(int offset, int limit, string orderBy, string searchText, string postId, bool? isConfirm);
    }
}
