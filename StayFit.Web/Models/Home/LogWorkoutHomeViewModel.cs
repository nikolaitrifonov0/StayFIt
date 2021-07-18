using System.Collections.Generic;

namespace StayFit.Web.Models.Home
{
    public class LogWorkoutHomeViewModel
    {
        public bool HasWorkout { get; init; }
        public bool IsWorkoutComplete { get; init; }
        public Dictionary<string, string> Exercises { get; init; }
    }
}
