using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevConnect.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int FirstUserId { get; set; }
        [Required]
        public int SecondUserId { get; set; }
        public List<Message>? Messages { get; set; }
    }
}
