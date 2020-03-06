using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindHorizon.Common;
using MindHorizon.Data.Contracts;
using MindHorizon.Entities;
using MindHorizon.ViewModels.Comments;
using MindHorizon.ViewModels.DynamicAccess;

namespace MindHorizon.Areas.Admin.Controllers
{
    [DisplayName("مدیریت نظرات")]
    public class CommentsController : BaseController
    {
        private readonly IUnitOfWork _uw;
        private readonly IMapper _mapper;
        private const string CommentNotFound = "دیدگاه یافت نشد.";

        public CommentsController(IUnitOfWork uw, IMapper mapper)
        {
            _uw = uw;
            _uw.CheckArgumentIsNull(nameof(_uw));

            _mapper = mapper;
            _mapper.CheckArgumentIsNull(nameof(_mapper));
        }


        [HttpGet, DisplayName("مشاهده نظرات")]
        [Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public IActionResult Index(string postId, bool? isConfirmed)
        {
            return View(nameof(Index) , new CommentViewModel { PostId = postId, IsConfirm = isConfirmed});
        }


        [HttpGet, DisplayName("دریافت نظرات")]
        [Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public async Task<IActionResult> GetComments(string search, string order, int offset, int limit, string sort, string newsId, bool? isConfirm)
        {
            List<CommentViewModel> comments;
            int total = _uw.BaseRepository<Comment>().CountEntities();
            if (!search.HasValue())
                search = "";

            if (limit == 0)
                limit = total;

            if (!newsId.HasValue())
                newsId = "";

            if (sort == "نام")
            {
                if (order == "asc")
                    comments = await _uw.CommentRepository.GetPaginateCommentsAsync(offset, limit, "Name", search, newsId, isConfirm);
                else
                    comments = await _uw.CommentRepository.GetPaginateCommentsAsync(offset, limit, "Name desc", search, newsId, isConfirm);
            }


            else if (sort == "ایمیل")
            {
                if (order == "asc")
                    comments = await _uw.CommentRepository.GetPaginateCommentsAsync(offset, limit, "Email", search, newsId, isConfirm);
                else
                    comments = await _uw.CommentRepository.GetPaginateCommentsAsync(offset, limit, "Email desc", search, newsId, isConfirm);
            }

            else if (sort == "تاریخ ارسال")
            {
                if (order == "asc")
                    comments = await _uw.CommentRepository.GetPaginateCommentsAsync(offset, limit, "PostageDateTime", search, newsId, isConfirm);
                else
                    comments = await _uw.CommentRepository.GetPaginateCommentsAsync(offset, limit, "PostageDateTime desc", search, newsId, isConfirm);
            }

            else
                comments = await _uw.CommentRepository.GetPaginateCommentsAsync(offset, limit, "PostageDateTime desc", search, newsId, isConfirm);

            if (search != "")
                total = comments.Count();

            return Json(new { total = total, rows = comments });
        }

        [HttpGet, DisplayName("ورود به صفحه حذف نظر")]
        [Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public async Task<IActionResult> Delete(string commentId)
        {
            if (!commentId.HasValue())
                ModelState.AddModelError(string.Empty,CommentNotFound);
            else
            {
                var comment = await _uw.BaseRepository<Comment>().FindByIdAsync(commentId);
                if (comment == null)
                    ModelState.AddModelError(string.Empty, CommentNotFound);
                else
                    return PartialView("_DeleteConfirmation", comment);
            }
            return PartialView("_DeleteConfirmation");
        }


        [HttpPost, ActionName("Delete"),DisplayName("حذف نظر")]
        [Authorize(Policy = ConstantPolicies.DynamicPermission)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Comment model)
        {
            if (model.CommentId == null)
                ModelState.AddModelError(string.Empty,CommentNotFound);
            else
            {
                var comment = await _uw.BaseRepository<Comment>().FindByIdAsync(model.CommentId);
                if (comment == null)
                    ModelState.AddModelError(string.Empty,CommentNotFound);
                else
                {
                    _uw.BaseRepository<Comment>().Delete(comment);
                    await _uw.Commit();
                    TempData["notification"] = DeleteSuccess;
                    return PartialView("_DeleteConfirmation", comment);
                }
            }
            return PartialView("_DeleteConfirmation");
        }



        [HttpGet, DisplayName(" نمایش تایید یا عدم تایید نظر")]
        [Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public async Task<IActionResult> ConfirmOrInconfirm(string commentId)
        {
            if (!commentId.HasValue())
                ModelState.AddModelError(string.Empty, CommentNotFound);
            else
            {
                var comment = await _uw.BaseRepository<Comment>().FindByIdAsync(commentId);
                if (comment == null)
                    ModelState.AddModelError(string.Empty, CommentNotFound);
                else
                    return PartialView("_ConfirmOrInconfirm", comment);
            }
            return PartialView("_ConfirmOrInconfirm");
        }


        [HttpPost, DisplayName("ذخیره تایید یا عدم تایید نظرات")]
        [Authorize(Policy = ConstantPolicies.DynamicPermission)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmOrInconfirm(Comment model)
        {
            if (model.CommentId == null)
                ModelState.AddModelError(string.Empty, CommentNotFound);
            else
            {
                var comment = await _uw.BaseRepository<Comment>().FindByIdAsync(model.CommentId);
                if (comment == null)
                    ModelState.AddModelError(string.Empty, CommentNotFound);
                else
                {
                    if (comment.IsConfirm)
                        comment.IsConfirm = false;
                    else
                        comment.IsConfirm = true;

                    _uw.BaseRepository<Comment>().Update(comment);
                    await _uw.Commit();
                    TempData["notification"] = OperationSuccess;
                    return PartialView("_ConfirmOrInconfirm", comment);
                }
            }
            return PartialView("_ConfirmOrInconfirm");
        }


        [HttpPost, ActionName("DeleteGroup"), DisplayName("حذف گروهی نظرات")]
        [Authorize(Policy = ConstantPolicies.DynamicPermission)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGroupConfirmed(string[] btSelectItem)
        {
            if (btSelectItem.Count() == 0)
                ModelState.AddModelError(string.Empty, "هیچ دیدگاهی برای حذف انتخاب نشده است.");
            else
            {
                foreach (var item in btSelectItem)
                {
                    var comment = await _uw.BaseRepository<Comment>().FindByIdAsync(item);
                    _uw.BaseRepository<Comment>().Delete(comment);
                }

                await _uw.Commit();
                TempData["notification"] = "حذف گروهی اطلاعات با موفقیت انجام شد.";
            }

            return PartialView("_DeleteGroup");
        }

        [HttpGet, DisplayName("ارسال نظر")]
        [Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public IActionResult SendComment(string parentCommentId, string postId)
        {
            return PartialView("_SendComment",new CommentViewModel(parentCommentId, postId));
        }

        [HttpPost, DisplayName("ذخیره ارسال نظر")]
        [Authorize(Policy = ConstantPolicies.DynamicPermission)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendComment(CommentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.PostageDateTime = DateTimeExtensions.DateTimeWithOutMilliSecends(DateTime.Now);
                viewModel.CommentId = StringExtensions.GenerateId(10);
                await _uw.BaseRepository<Comment>().CreateAsync(_mapper.Map<Comment>(viewModel));
                await _uw.Commit();
                TempData["notification"] = "دیدگاه شما با موفقیت ارسال شد و بعد از تایید در سایت نمایش داده می شود.";
            }
            return PartialView("_SendComment", viewModel);
        }
    }
}