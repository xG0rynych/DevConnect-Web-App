using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevConnect.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; } = null!;
        [Required]
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public int? ArticleId { get; set; }
        public int? QuestionId { get; set; }
        [Required]
        public DateOnly CreatedAt { get; set; }

        public User Author { get; set; } = null!;
    }
}
