using Microsoft.AspNetCore.SignalR;

namespace SignalRDemo.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            //发送给所有客户端
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
