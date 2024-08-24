using DevConnect.Models;

namespace DevConnect.ViewModels
{
    public class UserIndexViewModel
    {
        public User CurrentUser { get; set; } = null!;
        public List<Article>? Articles { get; set; }
        public List<Question>? Questions { get; set; }
    }
}
