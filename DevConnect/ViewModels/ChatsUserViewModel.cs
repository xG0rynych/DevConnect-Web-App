using DevConnect.Models;

namespace DevConnect.ViewModels
{
    public class ChatsUserViewModel
    {
        public List<string>? Users { get; set; }
        public List<int>? ChatsId { get; set; }
        public List<string>? LastMessage { get; set; }
        public List<DateTime>? SendAt { get; set; }
    }
}
