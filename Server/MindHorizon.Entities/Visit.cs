using System;
using System.Collections.Generic;
using System.Text;

namespace MindHorizon.Entities
{
    public class Visit
    {
        public string PostId { get; set; }
        public string IpAddress { get; set; }
        public DateTime LastVisitDateTime { get; set; }
        public int NumberOfVisit { get; set; }

        public virtual Post Post { get; set; }
    }
}
