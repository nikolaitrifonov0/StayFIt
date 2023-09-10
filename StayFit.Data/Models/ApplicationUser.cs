using Microsoft.AspNetCore.Identity;
using System;

namespace StayFit.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime? NextWorkout { get; set; }
        public string NextWorkDayId { get; set; }
        public WorkDay NextWorkDay { get; set; }
    }
}
