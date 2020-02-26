using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MindHorizon.Common;
using MindHorizon.Data.Contracts;
using MindHorizon.Entities;
using MindHorizon.ViewModels.Comments;
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

        public int CountPostsPublished() => _context.Post.Where(n => n.IsPublish == true && n.PublishDateTime <= DateTime.Now).Count();
        public List<PostViewModel> GetPaginatePosts(int offset, int limit, Func<IGrouping<string, PostViewModel>, Object> orderByAscFunc, Func<IGrouping<string, PostViewModel>, Object> orderByDescFunc, string searchText,bool? isPublish)
        {
            string NameOfCategories = "";
            string NameOfTags = "";
            List<PostViewModel> postViewModel = new List<PostViewModel>();

            var postGroup = (from n in _context.Post.Include(v => v.Visits).Include(l => l.Likes).Include(u=>u.User).Include(c=>c.Comments)
                                   join e in _context.PostCategories on n.PostId equals e.PostId into bc
                                   from bct in bc.DefaultIfEmpty()
                                   join c in _context.Categories on bct.CategoryId equals c.CategoryId into cg
                                   from cog in cg.DefaultIfEmpty()
                                   join a in _context.PostTags on n.PostId equals a.PostId into ac
                                   from act in ac.DefaultIfEmpty()
                                   join t in _context.Tags on act.TagId equals t.TagId into tg
                                   from tog in tg.DefaultIfEmpty()
                                   where (n.Title.Contains(searchText) && isPublish==null?(n.IsPublish==true || n.IsPublish==false) : (isPublish==true?n.IsPublish==true && n.PublishDateTime<=DateTime.Now : n.IsPublish==false) )
                                   select (new PostViewModel
                                   {
                                       PostId = n.PostId,
                                       Title= n.Title,
                                       Abstract= n.Abstract,
                                       ShortTitle = n.Title.Length > 50 ? n.Title.Substring(0, 50) + "..." : n.Title,
                                       Url= n.Url,
                                       ImageName= n.ImageName,
                                       Description= n.Description,
                                       NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),
                                       NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),
                                       NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),
                                       NumberOfComments=n.Comments.Count(),
                                       NameOfCategories = cog != null ? cog.CategoryName : "",
                                       NameOfTags= tog!=null ? tog.TagName :"",
                                       AuthorName=n.User.FirstName+" "+ n.User.LastName,
                                       IsPublish= n.IsPublish,
                                       PublishDateTime=n.PublishDateTime==null? new DateTime(01,01,01):n.PublishDateTime,
                                       PersianPublishDate = n.PublishDateTime==null?"-": n.PublishDateTime.ConvertMiladiToShamsi("yyyy/MM/dd ساعت HH:mm:ss"),
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
                    Status = item.PostGroup.First().IsPublish==false?"پیش نویس": (item.PostGroup.First().PublishDateTime > DateTime.Now ? "انتشار در آینده" : "منتشر شده"),
                    NameOfCategories = NameOfCategories,
                    NameOfTags = NameOfTags,
                    ImageName= item.PostGroup.First().ImageName,
                    AuthorName = item.PostGroup.First().AuthorName,
                    NumberOfComments=item.PostGroup.First().NumberOfComments,
                    PublishDateTime= item.PostGroup.First().PublishDateTime,
                };
                postViewModel.Add(post);
            }

            foreach (var item in postViewModel)
                item.Row = ++offset;

            return postViewModel;

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

        public async Task<List<PostViewModel>> MostViewedPosts(int offset, int limit, string duration)
        {
            string NameOfCategories = "";
            List<PostViewModel> postViewModel = new List<PostViewModel>();
            DateTime StartMiladiDate;
            DateTime EndMiladiDate = DateTime.Now;

            if (duration == "week")
            {
                int NumOfWeek = ConvertDateTime.ConvertMiladiToShamsi(DateTime.Now, "dddd").GetNumOfWeek();
                StartMiladiDate = DateTime.Now.AddDays((-1) * NumOfWeek).Date + new TimeSpan(0, 0, 0);
            }

            else if (duration == "day")
                StartMiladiDate = DateTime.Now.Date + new TimeSpan(0, 0, 0);

            else
            {
                string DayOfMonth = ConvertDateTime.ConvertMiladiToShamsi(DateTime.Now, "dd").Fa2En();
                StartMiladiDate = DateTime.Now.AddDays((-1) * (int.Parse(DayOfMonth) - 1)).Date + new TimeSpan(0, 0, 0);
            }

            var postGroup = await (from n in _context.Post.Include(v => v.Visits).Include(l => l.Likes).Include(c => c.Comments)
                                   join e in _context.PostCategories on n.PostId equals e.PostId into bc
                                   from bct in bc.DefaultIfEmpty()
                                   join c in _context.Categories on bct.CategoryId equals c.CategoryId into cg
                                   from cog in cg.DefaultIfEmpty()
                                   where (n.PublishDateTime <= EndMiladiDate && StartMiladiDate <= n.PublishDateTime)
                                   select (new
                                   {
                                       n.PostId,
                                       ShortTitle = n.Title.Length > 60 ? n.Title.Substring(0, 60) + "..." : n.Title,
                                       n.Url,
                                       NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),
                                       NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),
                                       NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),
                                       NumberOfComments = n.Comments.Count(),
                                       n.ImageName,
                                       CategoryName = cog != null ? cog.CategoryName : "",
                                       PublishDateTime = n.PublishDateTime == null ? new DateTime(01, 01, 01) : n.PublishDateTime,
                                   })).GroupBy(b => b.PostId).Select(g => new { PostId = g.Key, PostGroup = g }).OrderByDescending(g => g.PostGroup.First().NumberOfVisit).Skip(offset).Take(limit).AsNoTracking().ToListAsync();

            foreach (var item in postGroup)
            {
                NameOfCategories = "";
                foreach (var a in item.PostGroup.Select(a => a.CategoryName).Distinct())
                {
                    if (NameOfCategories == "")
                        NameOfCategories = a;
                    else
                        NameOfCategories = NameOfCategories + " - " + a;
                }

                PostViewModel post = new PostViewModel()
                {
                    PostId = item.PostId,
                    ShortTitle = item.PostGroup.First().ShortTitle,
                    Url = item.PostGroup.First().Url,
                    NumberOfVisit = item.PostGroup.First().NumberOfVisit,
                    NumberOfDisLike = item.PostGroup.First().NumberOfDisLike,
                    NumberOfLike = item.PostGroup.First().NumberOfLike,
                    NameOfCategories = NameOfCategories,
                    PublishDateTime = item.PostGroup.First().PublishDateTime,
                    ImageName = item.PostGroup.First().ImageName,
                };
                postViewModel.Add(post);
            }

            return postViewModel;
        }


        public async Task<List<PostViewModel>> MostTalkPosts(int offset, int limit, string duration)
        {
            DateTime StartMiladiDate;
            DateTime EndMiladiDate = DateTime.Now;

            if (duration == "week")
            {
                int NumOfWeek = ConvertDateTime.ConvertMiladiToShamsi(DateTime.Now, "dddd").GetNumOfWeek();
                StartMiladiDate = DateTime.Now.AddDays((-1) * NumOfWeek).Date + new TimeSpan(0, 0, 0);
            }

            else if (duration == "day")
                StartMiladiDate = DateTime.Now.Date + new TimeSpan(0, 0, 0);

            else
            {
                string DayOfMonth = ConvertDateTime.ConvertMiladiToShamsi(DateTime.Now, "dd").Fa2En();
                StartMiladiDate = DateTime.Now.AddDays((-1) * (int.Parse(DayOfMonth) - 1)).Date + new TimeSpan(0, 0, 0);
            }

            return await (from n in _context.Post.Include(v => v.Visits).Include(l => l.Likes).Include(c => c.Comments)
                          where (n.PublishDateTime <= EndMiladiDate && StartMiladiDate <= n.PublishDateTime)
                          select (new PostViewModel
                          {
                              PostId = n.PostId,
                              ShortTitle = n.Title.Length > 50 ? n.Title.Substring(0, 50) + "..." : n.Title,
                              Url = n.Url,
                              NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),
                              NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),
                              NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),
                              NumberOfComments = n.Comments.Count(),
                              ImageName = n.ImageName,
                              PublishDateTime = n.PublishDateTime == null ? new DateTime(01, 01, 01) : n.PublishDateTime,
                          })).OrderByDescending(o => o.NumberOfComments).Skip(offset).Take(limit).AsNoTracking().ToListAsync();
        }

        public async Task<List<PostViewModel>> MostPopularPosts(int offset, int limit)
        {
            return await (from n in _context.Post.Include(v => v.Visits).Include(l => l.Likes).Include(c => c.Comments)
                          where (n.IsPublish == true && n.PublishDateTime <= DateTime.Now)
                          select (new PostViewModel
                          {
                              PostId = n.PostId,
                              ShortTitle = n.Title.Length > 50 ? n.Title.Substring(0, 50) + "..." : n.Title,
                              Url = n.Url,
                              Title = n.Title,
                              NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),
                              NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),
                              NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),
                              NumberOfComments = n.Comments.Count(),
                              ImageName = n.ImageName,
                              PublishDateTime = n.PublishDateTime == null ? new DateTime(01, 01, 01) : n.PublishDateTime,
                          })).OrderByDescending(o => o.NumberOfLike).Skip(offset).Take(limit).AsNoTracking().ToListAsync();

        }

        public async Task<PostViewModel> GetPostById(string postId)
        {
            string NameOfCategories = "";
            var postGroup = await (from n in _context.Post.Include(v => v.Visits).Include(l => l.Likes).Include(u => u.User).Include(c => c.Comments)
                             join e in _context.PostCategories on n.PostId equals e.PostId into bc
                             from bct in bc.DefaultIfEmpty()
                             join c in _context.Categories on bct.CategoryId equals c.CategoryId into cg
                             from cog in cg.DefaultIfEmpty()
                             join a in _context.PostTags on n.PostId equals a.PostId into ac
                             from act in ac.DefaultIfEmpty()
                             join t in _context.Tags on act.TagId equals t.TagId into tg
                             from tog in tg.DefaultIfEmpty()
                             where (n.PostId == postId)
                             select (new PostViewModel
                             {
                                 PostId = n.PostId,
                                 Title = n.Title,
                                 Abstract = n.Abstract,
                                 ShortTitle = n.Title.Length > 50 ? n.Title.Substring(0, 50) + "..." : n.Title,
                                 Url = n.Url,
                                 ImageName = n.ImageName,
                                 Description = n.Description,
                                 NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),
                                 NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),
                                 NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),
                                 NumberOfComments = n.Comments.Where(c=>c.IsConfirm==true).Count(),
                                 NameOfCategories = cog != null ? cog.CategoryName : "",
                                 NameOfTags = tog != null ? tog.TagName : "",
                                 IdOfTags = tog != null ? tog.TagId : "",
                                 AuthorInfo =n.User,
                                 IsPublish = n.IsPublish,
                                 PublishDateTime = n.PublishDateTime == null ? new DateTime(01, 01, 01) : n.PublishDateTime,
                                 PersianPublishDate = n.PublishDateTime == null ? "-" : n.PublishDateTime.ConvertMiladiToShamsi("yyyy/MM/dd ساعت HH:mm:ss"),
                             })).GroupBy(b => b.PostId).Select(g => new { PostId = g.Key, PostGroup = g }).AsNoTracking().ToListAsync();


            foreach (var a in postGroup.First().PostGroup.Select(a => a.NameOfCategories).Distinct())
            {
                if (NameOfCategories == "")
                    NameOfCategories = a;
                else
                    NameOfCategories = NameOfCategories + " - " + a;
            }

            var post = new PostViewModel()
            {
                PostId = postGroup.First().PostGroup.First().PostId,
                Title = postGroup.First().PostGroup.First().Title,
                ShortTitle = postGroup.First().PostGroup.First().ShortTitle,
                Abstract = postGroup.First().PostGroup.First().Abstract,
                Url = postGroup.First().PostGroup.First().Url,
                Description = postGroup.First().PostGroup.First().Description,
                NumberOfVisit = postGroup.First().PostGroup.First().NumberOfVisit,
                NumberOfDisLike = postGroup.First().PostGroup.First().NumberOfDisLike,
                NumberOfLike = postGroup.First().PostGroup.First().NumberOfLike,
                PersianPublishDate = postGroup.First().PostGroup.First().PersianPublishDate,
                Status = postGroup.First().PostGroup.First().IsPublish == false ? "پیش نویس" : (postGroup.First().PostGroup.First().PublishDateTime > DateTime.Now ? "انتشار در آینده" : "منتشر شده"),
                NameOfCategories = NameOfCategories,
                TagNamesList = postGroup.First().PostGroup.Select(a=>a.NameOfTags).Distinct().ToList(),
                TagIdsList = postGroup.First().PostGroup.Select(a => a.IdOfTags).Distinct().ToList(),
                ImageName = postGroup.First().PostGroup.First().ImageName,
                AuthorInfo= postGroup.First().PostGroup.First().AuthorInfo,
                NumberOfComments = postGroup.First().PostGroup.First().NumberOfComments,
                PublishDateTime = postGroup.First().PostGroup.First().PublishDateTime,
            };

            return post;
        }

        public async Task<List<PostViewModel>> GetNextAndPreviousPost(DateTime? PublishDateTime)
        {
            var postList = new List<PostViewModel>();
            postList.Add( await (from n in _context.Post.Include(v => v.Visits).Include(l => l.Likes).Include(c => c.Comments)
                          where (n.IsPublish == true && n.PublishDateTime <= DateTime.Now && n.PublishDateTime< PublishDateTime)
                          select (new PostViewModel{PostId = n.PostId,ShortTitle = n.Title.Length > 50 ? n.Title.Substring(0, 50) + "..." : n.Title,Url = n.Url,Title = n.Title,NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),NumberOfComments = n.Comments.Count(),ImageName = n.ImageName,PublishDateTime = n.PublishDateTime == null ? new DateTime(01, 01, 01) : n.PublishDateTime,
                          })).OrderByDescending(o => o.PublishDateTime).AsNoTracking().FirstOrDefaultAsync());

            postList.Add(await (from n in _context.Post.Include(v => v.Visits).Include(l => l.Likes).Include(c => c.Comments)
                                where (n.IsPublish == true && n.PublishDateTime <= DateTime.Now && n.PublishDateTime > PublishDateTime)
                                select (new PostViewModel{PostId = n.PostId,ShortTitle = n.Title.Length > 50 ? n.Title.Substring(0, 50) + "..." : n.Title,Url = n.Url,Title = n.Title,NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),NumberOfComments = n.Comments.Count(),ImageName = n.ImageName,PublishDateTime = n.PublishDateTime == null ? new DateTime(01, 01, 01) : n.PublishDateTime,
                                })).OrderBy(o => o.PublishDateTime).AsNoTracking().FirstOrDefaultAsync());

            return postList;
        }

        public async Task<List<Comment>> GetPostCommentsAsync(string postId)
        {
            var comments = await (from c in _context.Comments
                                    where (c.ParentCommentId == null && c.PostId== postId && c.IsConfirm==true)
                                    select new Comment { CommentId = c.CommentId, Desription = c.Desription, Email=c.Email , PostageDateTime=c.PostageDateTime, Name = c.Name , PostId=c.PostId }).ToListAsync();
            foreach (var item in comments)
                await BindSubComments(item);

            return comments;
        }

        public async Task BindSubComments(Comment comment)
        {
            var subComments = await (from c in _context.Comments
                                 where (c.ParentCommentId == comment.CommentId && c.IsConfirm==true)
                                 select new Comment { CommentId = c.CommentId, Desription = c.Desription, Email = c.Email, PostageDateTime = c.PostageDateTime ,Name=c.Name, PostId = c.PostId }).ToListAsync();

            foreach (var item in subComments)
            {
                await BindSubComments(item);
                comment.comments.Add(item);
            }
        }
        public async Task<List<PostViewModel>> GetRelatedPosts(int number, List<string> tagIdList,string postId)
        {
            var postList = new List<PostViewModel>();
            int randomRow;
            int postCount = _context.Post.Include(t => t.PostTags).Where(n => n.IsPublish == true && n.PublishDateTime <= DateTime.Now && tagIdList.Any(y => n.PostTags.Select(x => x.TagId).Contains(y)) && n.PostId != postId).Count();
            for (int i = 0; i < number && i< postCount; i++)
            {
                randomRow = CustomMethods.RandomNumber(1, postCount + 1);
                var post = await _context.Post.Include(t => t.PostTags).Include(c=>c.Comments).Include(l=>l.Likes).Include(l => l.Visits).Where(n => n.IsPublish == true && n.PublishDateTime <= DateTime.Now && tagIdList.Any(y => n.PostTags.Select(x => x.TagId).Contains(y)) && n.PostId != postId)
                    .Select(n => new PostViewModel
                    { Title = n.Title, Url = n.Url, PostId = n.PostId, ImageName = n.ImageName, PublishDateTime=n.PublishDateTime,
                        NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),
                        NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),
                        NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),
                        NumberOfComments = n.Comments.Count(),
                    })
                    .Skip(randomRow - 1).Take(1).FirstOrDefaultAsync();

                postList.Add(post);
            }

            return postList;
        }


        public async Task<List<PostViewModel>> GetPostsInCategoryAndTag(string categoryId,string TagId)
        {
            string NameOfCategories = "";
            List<PostViewModel> postViewModel = new List<PostViewModel>();
            var postGroup = await (from n in _context.Post.Include(v => v.Visits).Include(l => l.Likes).Include(u => u.User).Include(c => c.Comments)
                                   join e in _context.PostCategories on n.PostId equals e.PostId into bc
                                   from bct in bc.DefaultIfEmpty()
                                   join c in _context.Categories on bct.CategoryId equals c.CategoryId into cg
                                   from cog in cg.DefaultIfEmpty()
                                   join a in _context.PostTags on n.PostId equals a.PostId into ac
                                   from act in ac.DefaultIfEmpty()
                                   join t in _context.Tags on act.TagId equals t.TagId into tg
                                   from tog in tg.DefaultIfEmpty()
                                   where (bct.CategoryId.Contains(categoryId) && act.TagId.Contains(TagId))
                                   select (new PostViewModel
                                   {
                                       PostId = n.PostId,
                                       Title = n.Title,
                                       Abstract = n.Abstract,
                                       ShortTitle = n.Title.Length > 50 ? n.Title.Substring(0, 50) + "..." : n.Title,
                                       Url = n.Url,
                                       ImageName = n.ImageName,
                                       Description = n.Description,
                                       NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),
                                       NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),
                                       NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),
                                       NumberOfComments = n.Comments.Where(c => c.IsConfirm == true).Count(),
                                       NameOfCategories = cog != null ? cog.CategoryName : "",
                                       NameOfTags = tog != null ? tog.TagName : "",
                                       IdOfTags = tog != null ? tog.TagId : "",
                                       AuthorName = n.User.FirstName+" "+ n.User.LastName,
                                       IsPublish = n.IsPublish,
                                       PublishDateTime = n.PublishDateTime == null ? new DateTime(01, 01, 01) : n.PublishDateTime,
                                       PersianPublishDate = n.PublishDateTime == null ? "-" : n.PublishDateTime.ConvertMiladiToShamsi("yyyy/MM/dd ساعت HH:mm:ss"),
                                   })).GroupBy(b => b.PostId).Select(g => new { PostId = g.Key, PostGroup = g }).AsNoTracking().ToListAsync();


            foreach (var item in postGroup)
            {
                NameOfCategories = "";
                foreach (var a in item.PostGroup.Select(a => a.NameOfCategories).Distinct())
                {
                    if (NameOfCategories == "")
                        NameOfCategories = a;
                    else
                        NameOfCategories = NameOfCategories + " - " + a;
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
                    ImageName = item.PostGroup.First().ImageName,
                    AuthorName = item.PostGroup.First().AuthorName,
                    NumberOfComments = item.PostGroup.First().NumberOfComments,
                    PublishDateTime = item.PostGroup.First().PublishDateTime,
                };
                postViewModel.Add(post);
            }
            return postViewModel;
        }


        public async Task<List<PostViewModel>> GetUserBookmarksAsync(int userId)
        {
            return await (from u in _context.Users
                          join b in _context.Bookmarks on u.Id equals b.UserId
                          join n in _context.Post on b.PostId equals n.PostId
                          where (u.Id == userId)
                          select new PostViewModel { PostId = n.PostId, Title = n.Title, PersianPublishDate = n.PublishDateTime.ConvertMiladiToShamsi("dd MMMM yyyy ساعت HH:mm"), Url = n.Url }).ToListAsync();

        }
    }
}
