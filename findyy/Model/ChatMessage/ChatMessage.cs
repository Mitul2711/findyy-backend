namespace findyy.Model
{
    public class ChatMessageModel
    {
        public int Id { get; set; }
        public int UnreadCount { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool Unread { get; set; }
    }

}
