namespace ReadNest.Application.Models.Requests.ChatMessage
{
    public class ChatMessageRequest
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Message { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
