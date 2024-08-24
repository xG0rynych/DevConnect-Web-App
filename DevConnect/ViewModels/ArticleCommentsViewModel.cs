using DevConnect.Models;

namespace DevConnect.ViewModels
{
    public class ArticleCommentsViewModel
    {
        public Article CurrentArticle { get; set; } = null!;
        public List<Comment>? Comments { get; set; }
    }
}
