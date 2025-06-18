using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadNest.Domain.Entities;

namespace ReadNest.Application.Services
{
    public interface IRedisChatQueue
    {
        Task EnqueueMessageAsync(ChatMessage message);
        Task<string> DequeueRawMessageAsync();
    }
}
