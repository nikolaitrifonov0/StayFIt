using System.Collections.Generic;

namespace StayFit.Web.Models.Users
{
    public class LogWorkoutViewModel
    {
        public string Name { get; set; }
        public bool HasWorkout { get; set; } = false;
        public bool IsWorkdayComplete { get; set; } = false;
        public Dictionary<string, string> Exercises { get; init; } = new();
    }
}
