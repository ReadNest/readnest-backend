﻿namespace ReadNest.Shared.Common
{
    public class ApiResponse<T> where T : class
    {
        public bool Success { get; set; }
        public string MessageId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<DetailError>? ListDetailError { get; set; }

        public static ApiResponse<T> Fail(string messageId) =>
            new()
            {
                Success = false,
                MessageId = messageId,
                Message = Common.Message.GetMessageById(messageId)
            };

        public static ApiResponse<T> Ok(T data, string messageId = Common.MessageId.I0000) =>
            new()
            {
                Success = true,
                MessageId = messageId,
                Message = Common.Message.GetMessageById(messageId),
                Data = data
            };
    }
}
