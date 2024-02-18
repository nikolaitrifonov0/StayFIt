using System;

namespace StayFit.Services.ChatLogs
{
    public class ChatLogViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
