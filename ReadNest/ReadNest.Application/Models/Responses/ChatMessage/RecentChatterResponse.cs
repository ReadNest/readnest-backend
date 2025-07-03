namespace ReadNest.Application.Models.Responses.ChatMessage
{
    public class RecentChatterResponse
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime LastMessageTime { get; set; } // LastMessage in the Conversation not the last message of the Chatter
        public int UnreadMessagesCount { get; set; }
        public string LastMessage { get; set; }
    }
}
