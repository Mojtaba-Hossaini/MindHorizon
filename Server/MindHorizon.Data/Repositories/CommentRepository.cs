using Microsoft.EntityFrameworkCore;
using MindHorizon.Common;
using MindHorizon.Data.Contracts;
using MindHorizon.ViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace MindHorizon.Data.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly MindHorizonDbContext _context;
        public CommentRepository(MindHorizonDbContext context)
        {
            _context = context;
        }

        public int CountUnAnsweredComments() => _context.Comments.Where(c => c.IsConfirm == false).Count();

        public async Task<List<CommentViewModel>> GetPaginateCommentsAsync(int offset, int limit, string orderBy, string searchText, string postId, bool? isConfirm)
        {

            var convertConfirm = Convert.ToBoolean(isConfirm);
            var getDateTimesForSearch = searchText.GetDateTimeForSearch();
            List<CommentViewModel> comments = await _context.Comments
                                   .Where(n => (isConfirm == null || (convertConfirm == true ? n.IsConfirm : !n.IsConfirm)) && n.PostId.Contains(postId) && (n.Name.Contains(searchText) || n.Email.Contains(searchText) || (n.PostageDateTime >= getDateTimesForSearch.First() && n.PostageDateTime <= getDateTimesForSearch.Last())))
                                   .OrderBy(orderBy)
                                   .Skip(offset).Take(limit)
                                   .Select(l => new CommentViewModel { CommentId = l.CommentId, Name = l.Name, Email = l.Email, IsConfirm = l.IsConfirm, PersianPostageDateTime = l.PostageDateTime.ConvertMiladiToShamsi("yyyy/MM/dd ساعت HH:mm:ss"), Desription = l.Desription })
                                   .AsNoTracking()
                                   .ToListAsync();

            foreach (var item in comments)
                item.Row = ++offset;

            return comments;
        }
    }
}
