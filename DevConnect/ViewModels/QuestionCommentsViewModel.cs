using DevConnect.Models;

namespace DevConnect.ViewModels
{
    public class QuestionCommentsViewModel
    {
        public Question CurrentQuestion { get; set; } = null!;
        public List<Comment>? Comments { get; set; }
    }
}
