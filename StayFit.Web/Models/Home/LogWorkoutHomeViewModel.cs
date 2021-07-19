using System.Collections.Generic;

namespace StayFit.Web.Models.Home
{
    public class LogWorkoutHomeViewModel
    {
        public bool HasWorkout { get; set; } = false;
        public bool IsWorkdayComplete { get; set; } = false;
        public Dictionary<string, string> Exercises { get; init; } = new();
    }
}
