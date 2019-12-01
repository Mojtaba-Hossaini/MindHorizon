using System;
using System.ComponentModel.DataAnnotations;

namespace MindHorizon.Entities
{
    public class PostLetter
    {
        [Key]
        public string Email { get; set; }
        public DateTime? RegisterDateTime { get; set; }
        public bool IsActive { get; set; }
    }
}
