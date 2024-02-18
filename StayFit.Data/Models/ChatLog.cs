using System;
using System.ComponentModel.DataAnnotations;

namespace StayFit.Data.Models
{
    public class ChatLog
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
