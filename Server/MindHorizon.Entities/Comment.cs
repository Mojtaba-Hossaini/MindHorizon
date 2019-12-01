using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MindHorizon.Entities
{
    public class Comment
    {
        [Key]
        public string CommentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Desription { get; set; }
        public string PostId { get; set; }
        public bool IsConfirm { get; set; }
        public DateTime? PostageDateTime { get; set; }

        [ForeignKey("comment")]
        public string ParentCommentId { get; set; }

        public virtual Comment comment { get; set; }
        public virtual ICollection<Comment> comments { get; set; }
        public virtual Post Post { get; set; }
    }
}
