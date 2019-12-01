using System.ComponentModel.DataAnnotations;

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
