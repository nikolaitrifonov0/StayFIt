using AutoMapper;
using AutoMapper.QueryableExtensions;
using StayFit.Data;
using StayFit.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StayFit.Services.ChatLogs
{
    public class ChatService : IChatService
    {
        private readonly StayFitContext data;
        private readonly IConfigurationProvider mapper;

        public ChatService(StayFitContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }
        public void CreateLog(ChatLogViewModel model)
        {
            var chatLog = new ChatLog
            {
                Message = model.Message,
                Timestamp = model.Timestamp,
                UserName = model.UserName
            };

            this.data.ChatLogs.Add(chatLog);
            this.data.SaveChanges();
        }

        public IEnumerable<ChatLogViewModel> ReadLogs(DateTime date)
        {
            return this.data.ChatLogs.Where(c => c.Timestamp.Date == date).ProjectTo<ChatLogViewModel>(this.mapper).OrderBy(c => c.Timestamp).ToList();
        }
    }
}
