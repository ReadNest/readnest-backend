using Microsoft.AspNetCore.SignalR;

namespace ReadNest.WebAPI.Hubs
{

    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.GetHttpContext()?.Request.Query["userId"];
        }
    }
}
