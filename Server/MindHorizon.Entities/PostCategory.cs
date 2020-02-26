using System;
using System.Collections.Generic;
using System.Text;

namespace MindHorizon.Entities
{
    public class PostCategory
    {
        public string PostId { get; set; }
        public string CategoryId { get; set; }

        public virtual Post Post { get; set; }
        public virtual Category Category { get; set; }
    }
}
