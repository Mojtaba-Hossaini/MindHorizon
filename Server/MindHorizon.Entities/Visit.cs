using System;

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
