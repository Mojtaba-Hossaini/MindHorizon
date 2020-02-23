using MindHorizon.Common;
using MindHorizon.Data;
using MindHorizon.Data.Contracts;
using MindHorizon.ViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public List<CommentViewModel> GetPaginateComments(int offset, int limit, Func<CommentViewModel, Object> orderByAscFunc, Func<CommentViewModel, Object> orderByDescFunc, string searchText)
        {
            List<CommentViewModel> comments = _context.Comments.Where(c => c.Name.Contains(searchText) || c.Email.Contains(searchText) || c.PostageDateTime.ConvertMiladiToShamsi("yyyy/MM/dd ساعت hh:mm:ss").Contains(searchText))
                                   .Select(l => new CommentViewModel {CommentId=l.CommentId,Name=l.Name , Email = l.Email, IsConfirm = l.IsConfirm, PersianPostageDateTime = l.PostageDateTime.ConvertMiladiToShamsi("yyyy/MM/dd ساعت hh:mm:ss") , Desription=l.Desription })
                                   .OrderBy(orderByAscFunc).OrderByDescending(orderByDescFunc)
                                   .Skip(offset).Take(limit).ToList();

            foreach (var item in comments)
                item.Row = ++offset;

            return comments;
        }
    }
}
