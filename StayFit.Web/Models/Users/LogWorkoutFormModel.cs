using System.Collections.Generic;

namespace StayFit.Web.Models.Users
{
    public class LogWorkoutFormModel
    {
        public string Name { get; set; }
        public bool HasWorkout { get; set; } = false;
        public bool IsWorkdayComplete { get; set; } = false;
        public Dictionary<string, string> Exercises { get; init; } = new();
        public IEnumerable<int> Set { get; init; }
        public IEnumerable<int> Reps { get; init; }
        public IEnumerable<int> Weight { get; init; }
    }
}
