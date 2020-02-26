using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MindHorizon.Entities
{
    public class PostImage
    {
        [Key]
        public string PostImageId { get; set; }
        public string PostId { get; set; }
        public string ImageName { get; set; }

        public virtual Post Post { get; set; }
    }
}
