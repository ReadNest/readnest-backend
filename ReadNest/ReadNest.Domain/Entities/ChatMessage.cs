using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class ChatMessage : BaseEntity<Guid>
    {
        public Guid SenderId { get; set; }
        public User Sender { get; set; }
        public Guid ReceiverId { get; set; }
        public User Receiver { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}
