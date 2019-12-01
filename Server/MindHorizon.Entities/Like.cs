namespace MindHorizon.Entities
{
    public class Like
    {
        public string PostId { get; set; }
        public string IpAddress { get; set; }
        public bool IsLiked { get; set; }

        public virtual Post Post { get; set; }
    }
}
