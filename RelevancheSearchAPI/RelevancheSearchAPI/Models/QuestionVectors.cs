using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RelevancheSearchAPI.Models
{
    [Index(nameof(QuestionId), IsUnique = true)]
    public class QuestionVectors
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public string Vector { get; set; } = null!;
    }
}
