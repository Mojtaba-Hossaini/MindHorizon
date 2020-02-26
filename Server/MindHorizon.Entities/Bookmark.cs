using MindHorizon.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MindHorizon.Entities
{
    public class Bookmark
    {
        public string PostId { get; set; }
        public int UserId { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
