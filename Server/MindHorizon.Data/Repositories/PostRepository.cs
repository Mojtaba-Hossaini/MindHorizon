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


        public  List<PostViewModel> GetPaginatePost(int offset, int limit, Func<IGrouping<string, PostViewModel>, object> orderByAscFunc, Func<IGrouping<string, PostViewModel>, object> orderByDescFunc, string searchText, bool? isPublish)
        {
            string NameOfCategories = "";
            string NameOfTags = "";
            List<PostViewModel> PostViewModel = new List<PostViewModel>();

            var postGroup = (from n in _context.Post.Include(v => v.Visits).Include(l => l.Likes).Include(u => u.User).Include(c => c.Comments)
                                   join e in _context.PostCategories on n.PostId equals e.PostId into bc
                                   from bct in bc.DefaultIfEmpty()
                                   join c in _context.Categories on bct.CategoryId equals c.CategoryId into cg
                                   from cog in cg.DefaultIfEmpty()
                                   join a in _context.PostTags on n.PostId equals a.PostId into ac
                                   from act in ac.DefaultIfEmpty()
                                   join t in _context.Tags on act.TagId equals t.TagId into tg
                                   from tog in tg.DefaultIfEmpty()
                                   where (n.Title.Contains(searchText) && isPublish == null ? (n.IsPublish == true || n.IsPublish == false) : (isPublish == true ? n.IsPublish == true && n.PublishDateTime<= DateTime.Now : n.IsPublish == false))
                                   select (new PostViewModel
                                   {
                                       PostId =  n.PostId,
                                       Title =  n.Title,
                                       Abstract =  n.Abstract,
                                       ShortTitle = n.Title.Length > 45 ? n.Title.Substring(0, 45) + "..." : n.Title,
                                       Url =  n.Url,
                                       ImageName = n.ImageName,
                                       Description = n.Description,
                                       NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),
                                       NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),
                                       NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),
                                       NumberOfComments = n.Comments.Count(),
                                       NameOfCategories = cog != null ? cog.CategoryName : "",
                                       NameOfTags = tog != null ? tog.TagName : "",
                                       AuthorName = n.User.FirstName + " " + n.User.LastName,
                                       IsPublish = n.IsPublish,
                                       PublishDateTime = n.PublishDateTime == null ? new DateTime(01, 01, 01) : n.PublishDateTime,
                                       PersianPublishDate = n.PublishDateTime == null ? "-" : n.PublishDateTime.ConvertMiladiToShamsi("yyyy/MM/dd ساعت HH:mm:ss"),
                                   })).GroupBy(b => b.PostId).OrderBy(orderByAscFunc).OrderByDescending(orderByDescFunc).Select(g => new { PostId = g.Key, PostGroup = g }).Skip(offset).Take(limit).ToList();

            foreach (var item in postGroup)
            {
                NameOfCategories = "";
                NameOfTags = "";
                foreach (var a in item.PostGroup.Select(a => a.NameOfCategories).Distinct())
                {
                    if (NameOfCategories == "")
                        NameOfCategories = a;
                    else
                        NameOfCategories = NameOfCategories + " - " + a;
                }

                foreach (var a in item.PostGroup.Select(a => a.NameOfTags).Distinct())
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
                    Abstract = item.PostGroup.First().Abstract,
                    Url = item.PostGroup.First().Url,
                    Description = item.PostGroup.First().Description,
                    NumberOfVisit = item.PostGroup.First().NumberOfVisit,
                    NumberOfDisLike = item.PostGroup.First().NumberOfDisLike,
                    NumberOfLike = item.PostGroup.First().NumberOfLike,
                    PersianPublishDate = item.PostGroup.First().PersianPublishDate,
                    Status = item.PostGroup.First().IsPublish == false ? "پیش نویس" : (item.PostGroup.First().PublishDateTime > DateTime.Now ? "انتشار در آینده" : "منتشر شده"),
                    NameOfCategories = NameOfCategories,
                    NameOfTags = NameOfTags,
                    ImageName = item.PostGroup.First().ImageName,
                    AuthorName = item.PostGroup.First().AuthorName,
                    NumberOfComments = item.PostGroup.First().NumberOfComments,
                    PublishDateTime = item.PostGroup.First().PublishDateTime
                };
                PostViewModel.Add(post);
            }

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
