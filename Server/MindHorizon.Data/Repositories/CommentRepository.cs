using MindHorizon.Common;
using MindHorizon.Data.Contracts;
using MindHorizon.Entities;
using MindHorizon.ViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

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

        public List<CommentViewModel> GetPaginateComments(int offset, int limit, Func<CommentViewModel, Object> orderByAscFunc, Func<CommentViewModel, Object> orderByDescFunc, string searchText, string newsId, bool? isConfirm)
        {
            Expression<Func<Comment, bool>> filter;
            if (isConfirm == null)
                filter = c => (c.Name.Contains(searchText) || c.Email.Contains(searchText) || c.PostageDateTime.ConvertMiladiToShamsi("yyyy/MM/dd ساعت hh:mm:ss").Contains(searchText) || c.IsConfirm == true || c.IsConfirm == false) && c.PostId.Contains(newsId);
            else
                filter = c => (c.Name.Contains(searchText) || c.Email.Contains(searchText) || c.PostageDateTime.ConvertMiladiToShamsi("yyyy/MM/dd ساعت hh:mm:ss").Contains(searchText)) && c.PostId.Contains(newsId) && isConfirm == true ? c.IsConfirm == true : c.IsConfirm == false;

            List<CommentViewModel> comments = _context.Comments.Where(filter)
                                   .Select(l => new CommentViewModel { CommentId = l.CommentId, Name = l.Name, Email = l.Email, IsConfirm = l.IsConfirm, PersianPostageDateTime = l.PostageDateTime.ConvertMiladiToShamsi("yyyy/MM/dd ساعت hh:mm:ss"), Desription = l.Desription })
                                   .OrderBy(orderByAscFunc).OrderByDescending(orderByDescFunc)
                                   .Skip(offset).Take(limit).ToList();

            foreach (var item in comments)
                item.Row = ++offset;

            return comments;
        }
    }
}
