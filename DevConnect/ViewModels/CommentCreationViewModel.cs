using DevConnect.Models;

namespace DevConnect.ViewModels
{
    public class CommentCreationViewModel
    {
        public Comment? CurrentComment { get; set; }
        public int? ArticleId { get; set; }
        public int? QuestionId { get; set; }
    }
}
