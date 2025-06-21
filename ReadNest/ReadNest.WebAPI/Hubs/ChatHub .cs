using Microsoft.AspNetCore.SignalR;
using ReadNest.Application.Models.Requests.ChatMessage;
using ReadNest.Application.Services;
using ReadNest.Domain.Entities;

namespace ReadNest.WebAPI.Hubs
{
    /// <summary>
    /// SignalR hub for managing real-time chat functionality.
    /// Handles receiving chat messages from clients, broadcasting them to all connected clients,
    /// and enqueuing messages into a Redis-backed queue for temporary storage.
    /// </summary>
    public class ChatHub : Hub
    {
        private readonly IRedisChatQueue _redisChatQueue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatHub"/> class.
        /// </summary>
        /// <param name="redisChatQueue">The Redis-backed chat queue for message storage.</param>
        public ChatHub(IRedisChatQueue redisChatQueue)
        {
            _redisChatQueue = redisChatQueue;
        }

        /// <summary>
        /// Handles sending a chat message from a client.
        /// Constructs a <see cref="ChatMessage"/> from the provided <see cref="ChatMessageRequest"/>,
        /// broadcasts it to all connected clients via SignalR, and enqueues the message in a Redis-backed queue
        /// for temporary storage.
        /// </summary>
        /// <param name="request">The chat message request containing sender, receiver, and message content.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendMessage(ChatMessageRequest request)
        {
            var message = new ChatMessageCacheModel
            {
                Id = request.Id,
                SenderId = request.SenderId,
                ReceiverId = request.ReceiverId,
                Message = request.Message,
                SentAt =DateTime.UtcNow, // Ensure a valid timestamp
                IsRead = false, // Default to unread 
            };
            // Broadcast gửi về cho all clients
            await Clients.All.SendAsync("ReceiveMessage", message);
            // Đẩy vào redisQueue lưu tạm thời
            await _redisChatQueue.AddMessageAsync(message, false);
        }
    }
}
