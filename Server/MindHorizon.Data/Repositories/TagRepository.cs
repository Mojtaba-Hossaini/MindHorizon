using Microsoft.EntityFrameworkCore;
using MindHorizon.Common;
using MindHorizon.Data.Contracts;
using MindHorizon.Entities;
using MindHorizon.ViewModels.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindHorizon.Data.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly MindHorizonDbContext _context;
        public TagRepository(MindHorizonDbContext context)
        {
            _context = context;
        }


        public async Task<List<TagViewModel>> GetPaginateTagsAsync(int offset, int limit, bool? tagNameSortAsc, string searchText)
        {
            List<TagViewModel> tags = await _context.Tags.Where(c => c.TagName.Contains(searchText))
                                   .Select(t => new TagViewModel {TagId=t.TagId,TagName=t.TagName}).Skip(offset).Take(limit).AsNoTracking().ToListAsync();

            if (tagNameSortAsc != null)
                tags = tags.OrderBy(c => (tagNameSortAsc == true && tagNameSortAsc != null) ? c.TagName : "").OrderByDescending(c => (tagNameSortAsc == false && tagNameSortAsc != null) ? c.TagName : "").ToList();

            foreach (var item in tags)
                item.Row = ++offset;

            return tags;
        }

        public bool IsExistTag(string tagName, string recentTagId = null)
        {
            if (!recentTagId.HasValue())
                return _context.Tags.Any(c => c.TagName.Trim().Replace(" ", "") == tagName.Trim().Replace(" ", ""));
            else
            {
                var tag = _context.Tags.Where(c => c.TagName.Trim().Replace(" ", "") == tagName.Trim().Replace(" ", "")).FirstOrDefault();
                if (tag == null)
                    return false;
                else
                {
                    if (tag.TagId != recentTagId)
                        return true;
                    else
                        return false;
                }
            }
        }


        public async Task<List<PostTag>> InsertPostTags(string[] tags,string postId=null)
        {
            string tagId;
            List<PostTag> postTags = new List<PostTag>();
            var allTags = _context.Tags.ToList();
            postTags.AddRange(allTags.Where(n => tags.Contains(n.TagName)).Select(c => new PostTag { TagId = c.TagId, PostId = postId }).ToList());
            var newTags = tags.Where(n => !allTags.Select(t => t.TagName).Contains(n)).ToList();
            foreach (var item in newTags)
            {
                tagId = StringExtensions.GenerateId(10);
                _context.Tags.Add(new Tag { TagName = item, TagId = tagId });
                postTags.Add(new PostTag { TagId = tagId,PostId= postId });
            }
            await _context.SaveChangesAsync();
            return postTags;
        }
    }
}
