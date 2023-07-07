using Microsoft.AspNetCore.SignalR;

namespace SaleSupportySystem
{
    public class MyHub : Hub
    {
        public async Task SendMessage(string message)
        {
            // 处理接收到的消息
            await Clients.All.SendAsync("broadcastMessage", message);
        }
    }
}
