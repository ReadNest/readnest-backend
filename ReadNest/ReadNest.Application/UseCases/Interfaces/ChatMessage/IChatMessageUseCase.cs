using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadNest.Application.Models.Requests.ChatMessage;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.ChatMessage
{
    public interface IChatMessageUseCase
    {
        Task<ApiResponse<String>> SaveRangeMessageAsync(List<Domain.Entities.ChatMessage> messages);
    }
}
