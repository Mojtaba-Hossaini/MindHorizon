using MindHorizon.Entities.Identity;

namespace MindHorizon.Entities
{
    public class Bookmark
    {
        public string PostId { get; set; }
        public string UserId { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
