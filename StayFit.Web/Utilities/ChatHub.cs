using Microsoft.AspNetCore.SignalR;
using StayFit.Services.ChatLogs;
using System;
using System.Threading.Tasks;

namespace StayFit.Web.Utilities
{
    public class ChatHub : Hub
    {
        private readonly IChatService chatService;

        public ChatHub(IChatService chatService)
        {
            this.chatService = chatService;
        }

        public async Task SendMessage(string user, string message)
        {
            chatService.CreateLog(new ChatLogViewModel { Message = message, Timestamp = DateTime.Now, UserName = user });
            await Clients.Others.SendAsync("ReceiveMessage", user, message);
        }
    }
}
