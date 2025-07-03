namespace ReadNest.Application.Models.Requests.ChatMessage
{
    public class ChatMessageCacheModel
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public required string Message { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public bool IsSaved { get; set; } // true = from DB; false = new pending
    }
}
