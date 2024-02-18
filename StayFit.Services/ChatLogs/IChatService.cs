using System;
using System.Collections.Generic;

namespace StayFit.Services.ChatLogs
{
    public interface IChatService
    {
        IEnumerable<ChatLogViewModel> ReadLogs(DateTime date);
        void CreateLog(ChatLogViewModel model);
    }
}
