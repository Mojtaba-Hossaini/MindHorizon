using MindHorizon.ViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Text;

namespace MindHorizon.Data.Contracts
{
    public interface ICommentRepository
    {
        int CountUnAnsweredComments();
        List<CommentViewModel> GetPaginateComments(int offset, int limit, Func<CommentViewModel, Object> orderByAscFunc, Func<CommentViewModel, Object> orderByDescFunc, string searchText, string postId, bool? isConfirmed);
    }
}
