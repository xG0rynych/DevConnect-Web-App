namespace DevConnect.ViewModels
{
    public class ChatViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// true - my message, false - friend message
        /// </summary>
        public List<ValueTuple<string,bool>>? Messages { get; set; }
        public List<DateTime>? SendAt { get; set; }
        public int FriendId { get; set; }
        public string FriendUsername { get; set; } = null!;
    }
}
