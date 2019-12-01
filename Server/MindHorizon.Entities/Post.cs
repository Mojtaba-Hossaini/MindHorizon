using MindHorizon.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MindHorizon.Entities
{
    public class Post
    {
        [Key]
        public string PostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? PublishDateTime { get; set; }
        public string UserId { get; set; }
        public string Url { get; set; }
        public string ImageName { get; set; }
        public bool IsPublish { get; set; }
        public bool IsInternal { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<PostCategory> PostCategories { get; set; }
        public virtual ICollection<Bookmark> Bookmarks { get; set; }
        public virtual ICollection<PostTag> PostTags { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Visit> Visits { get; set; }
        public virtual ICollection<PostImage> PostImages { get; set; }
    }
}
