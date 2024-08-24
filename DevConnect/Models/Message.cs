using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevConnect.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("CurrentChat")]
        public int ChatId { get; set; }
        [Required]
        public int FromUserId { get; set; }
        [Required]
        public int ToUserId { get; set; }
        public string Content { get; set; } = null!;
        [Required]
        public DateTime SendAt { get; set; }

        public Chat CurrentChat { get; set; } = null!;
    }
}
