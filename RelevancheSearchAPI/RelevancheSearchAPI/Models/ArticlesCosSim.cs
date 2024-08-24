using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RelevancheSearchAPI.Models
{
    public class ArticlesCosSim
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ArticleId1 { get; set; }
        [Required]
        public int ArticleId2 { get; set; }
        [Required]
        public double CosSimilarity { get; set; }
    }
}
