using System;
using System.Collections.Generic;
using System.Text;

namespace MindHorizon.Entities
{
    public class PostTag
    {
        public string PostId { get; set; }
        public string TagId { get; set; }

        public Post Post { get; set; }
        public Tag Tag { get; set; }

    }
}
