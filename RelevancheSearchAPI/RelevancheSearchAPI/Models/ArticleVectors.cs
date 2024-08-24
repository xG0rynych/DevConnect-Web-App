using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace RelevancheSearchAPI.Models
{
    [Index(nameof(ArticleId), IsUnique = true)]
    public class ArticleVectors
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ArticleId { get; set; }
        [Required]
        public string Vector { get; set; } = null!;
    }
}
