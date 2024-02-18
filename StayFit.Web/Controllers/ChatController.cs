using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayFit.Services.ChatLogs;
using System;

namespace StayFit.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService chatService;

        public ChatController(IChatService chatService)
        {
            this.chatService = chatService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult ReadLogs()
        {
            return Ok(this.chatService.ReadLogs(DateTime.Today));
        }
    }
}
