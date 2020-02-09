using System;
using System.ComponentModel.DataAnnotations;

namespace MindHorizon.Entities
{
    public class Video
    {
        [Key]
        public string VideoId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Poster { get; set; }
        public DateTime? PublishDateTime { get; set; }
    }
}
