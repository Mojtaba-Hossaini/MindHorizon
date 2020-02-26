using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MindHorizon.Entities
{
    public class Tag
    {
        [Key]
        public string TagId { get; set; }
        public string TagName { get; set; }

        public virtual ICollection<PostTag> PostTags { get; set; }
    }
}
