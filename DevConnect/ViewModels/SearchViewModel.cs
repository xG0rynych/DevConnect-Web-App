using DevConnect.Models;
using DevConnect.Models.Enums;

namespace DevConnect.ViewModels
{
    public class SearchViewModel
    {
        public ModelTypeEnum Type { get; set; }
        public List<Article>? Articles { get; set; }
        public List<Question>? Questions { get; set; }
        public User? FoundUser { get; set; }
    }
}
