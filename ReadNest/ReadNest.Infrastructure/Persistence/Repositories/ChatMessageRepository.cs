using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class ChatMessageRepository(AppDbContext context) : GenericRepository<ChatMessage, Guid>(context), IChatMessageRepository
    {
        /// <summary>
        /// Retrieves all users who have sent chat messages to the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to find chatters for.</param>
        /// <returns>
        /// A collection of <see cref="User"/> entities who have sent messages to the specified user.
        /// </returns>
        public async Task<IEnumerable<User>> GetAllChattersByUserIdAsync(Guid userId)
        {
            //var users = await _context.Users
            //    .Include(u => u.SentMessages)
            //    .Where(u => u.SentMessages.Any(m => m.ReceiverId == userId)) // Get users who have sent messages to the specified user
            //    .ToListAsync();

            var userIds = await _context.ChatMessages
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .Select(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                .Distinct()
                .ToListAsync();

            return await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();
        }

        public async Task<User?> GetUserWhoSentMessageToAsync(Guid receiverId, string senderUsername)
        {
            //Get Chatter who sent message to reveiverId and have username is senderUsername
            return await _context.Users
                .Include(u => u.SentMessages)
                .FirstOrDefaultAsync(u =>
                    u.UserName == senderUsername
                );
        }

        public async Task<IEnumerable<ChatMessage>> GetFullConversationAsync(Guid userAId, Guid userBId)
        {
            return await _context.ChatMessages
                .Where(m => (m.SenderId == userAId && m.ReceiverId == userBId) || (m.SenderId == userBId && m.ReceiverId == userAId))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<User?> GetUserWhoSendMessageToByIdAsync(Guid senderId)
        {
            return await _context.Users
                .Include(u => u.SentMessages)
                .FirstOrDefaultAsync(u =>
                    u.Id == senderId
                );
        }
    }
}
