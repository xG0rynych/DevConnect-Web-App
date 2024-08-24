using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RelevancheSearchAPI.Models
{
    public class QuestionsCosSim
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int QuestionId1 { get; set; }
        [Required]
        public int QuestionId2 { get; set; }
        [Required]
        public double CosSimilarity { get; set; }
    }
}
