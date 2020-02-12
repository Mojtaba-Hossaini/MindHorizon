using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MindHorizon.Common;
using MindHorizon.Data.Contracts;
using MindHorizon.ViewModels.Post;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindHorizon.Data.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly MindHorizonDbContext _context;
        private readonly IMapper _mapper;
        public PostRepository(MindHorizonDbContext context, IMapper mapper)
        {
            _context = context;
            _context.CheckArgumentIsNull(nameof(_context));

            _mapper = mapper;
            _mapper.CheckArgumentIsNull(nameof(_mapper));
        }


        public async Task<List<PostViewModel>> GetPaginatePostAsync(int offset, int limit, bool? titleSortAsc, bool? visitSortAsc, bool? likeSortAsc, bool? dislikeSortAsc, bool? publishDateTimeSortAsc, string searchText)
        {
            string NameOfCategories = "";
            string NameOfTags = "";
            List<PostViewModel> PostViewModel = new List<PostViewModel>();

            var postGroup = await (from n in _context.Post.Include(v => v.Visits).Include(l => l.Likes).Include(u => u.User)
                                   join e in _context.PostCategories on n.PostId equals e.PostId into bc
                                   from bct in bc.DefaultIfEmpty()
                                   join c in _context.Categories on bct.CategoryId equals c.CategoryId into cg
                                   from cog in cg.DefaultIfEmpty()
                                   join a in _context.PostTags on n.PostId equals a.PostId into ac
                                   from act in ac.DefaultIfEmpty()
                                   join t in _context.Tags on act.TagId equals t.TagId into tg
                                   from tog in tg.DefaultIfEmpty()
                                   where (n.Title.Contains(searchText))
                                   select (new
                                   {
                                       n.PostId,
                                       n.Title,
                                       ShortTitle = n.Title.Length > 60 ? n.Title.Substring(0, 60) + "..." : n.Title,
                                       n.Url,
                                       n.Description,
                                       NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),
                                       NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),
                                       NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),
                                       CategoryName = cog != null ? cog.CategoryName : "",
                                       TagName = tog != null ? tog.TagName : "",
                                       AuthorName = n.User.FirstName + " " + n.User.LastName,
                                       n.IsPublish,
                                       PostType = n.IsInternal == true ? "داخلی" : "خارجی",
                                       PublishDateTime = n.PublishDateTime == null ? new DateTime(01, 01, 01) : n.PublishDateTime,
                                       PersianPublishDateTime = n.PublishDateTime == null ? "-" : n.PublishDateTime.ConvertMiladiToShamsi("yyyy/MM/dd ساعت hh:mm:ss"),
                                   })).GroupBy(b => b.PostId).Select(g => new { PostId = g.Key, PostGroup = g }).Skip(offset).Take(limit).AsNoTracking().ToListAsync();

            foreach (var item in postGroup)
            {
                NameOfCategories = "";
                NameOfTags = "";
                foreach (var a in item.PostGroup.Select(a => a.CategoryName).Distinct())
                {
                    if (NameOfCategories == "")
                        NameOfCategories = a;
                    else
                        NameOfCategories = NameOfCategories + " - " + a;
                }

                foreach (var a in item.PostGroup.Select(a => a.TagName).Distinct())
                {
                    if (NameOfTags == "")
                        NameOfTags = a;
                    else
                        NameOfTags = NameOfTags + " - " + a;
                }

                PostViewModel post = new PostViewModel()
                {
                    PostId = item.PostId,
                    Title = item.PostGroup.First().Title,
                    ShortTitle = item.PostGroup.First().ShortTitle,
                    Url = item.PostGroup.First().Url,
                    Description = item.PostGroup.First().Description,
                    NumberOfVisit = item.PostGroup.First().NumberOfVisit,
                    NumberOfDisLike = item.PostGroup.First().NumberOfDisLike,
                    NumberOfLike = item.PostGroup.First().NumberOfLike,
                    PersianPublishDate = item.PostGroup.First().PersianPublishDateTime,
                    Status = item.PostGroup.First().IsPublish == false ? "پیش نویس" : (item.PostGroup.First().PublishDateTime > DateTime.Now ? "انتشار در آینده" : "منتشر شده"),
                    NameOfCategories = NameOfCategories,
                    NameOfTags = NameOfTags,
                    AuthorName = item.PostGroup.First().AuthorName,
                };
                PostViewModel.Add(post);
            }

            if (titleSortAsc != null)
                PostViewModel = PostViewModel.OrderBy(c => (titleSortAsc == true && titleSortAsc != null) ? c.Title : "")
                                     .OrderByDescending(c => (titleSortAsc == false && titleSortAsc != null) ? c.Title : "").ToList();

            else if (visitSortAsc != null)
                PostViewModel = PostViewModel.OrderBy(c => (visitSortAsc == true && visitSortAsc != null) ? c.NumberOfVisit : 0)
                                   .OrderByDescending(c => (visitSortAsc == false && visitSortAsc != null) ? c.NumberOfVisit : 0).ToList();

            else if (likeSortAsc != null)
                PostViewModel = PostViewModel.OrderBy(c => (likeSortAsc == true && likeSortAsc != null) ? c.NumberOfLike : 0)
                                   .OrderByDescending(c => (likeSortAsc == false && likeSortAsc != null) ? c.NumberOfLike : 0).ToList();

            else if (dislikeSortAsc != null)
                PostViewModel = PostViewModel.OrderBy(c => (dislikeSortAsc == true && dislikeSortAsc != null) ? c.NumberOfDisLike : 0)
                                   .OrderByDescending(c => (dislikeSortAsc == false && dislikeSortAsc != null) ? c.NumberOfDisLike : 0).ToList();

            else if (publishDateTimeSortAsc != null)
                PostViewModel = PostViewModel.OrderBy(c => (publishDateTimeSortAsc == true && publishDateTimeSortAsc != null) ? c.PersianPublishDate : "")
                                   .OrderByDescending(c => (publishDateTimeSortAsc == false && publishDateTimeSortAsc != null) ? c.PersianPublishDate : "").ToList();

            foreach (var item in PostViewModel)
                item.Row = ++offset;

            return PostViewModel;

        }

        public string CheckPostFileName(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName);
            int fileNameCount = _context.Post.Where(f => f.ImageName == fileName).Count();
            int j = 1;
            while (fileNameCount != 0)
            {
                fileName = fileName.Replace(fileExtension, "") + j + fileExtension;
                fileNameCount = _context.Videos.Where(f => f.Poster == fileName).Count();
                j++;
            }

            return fileName;
        }
    }
}
