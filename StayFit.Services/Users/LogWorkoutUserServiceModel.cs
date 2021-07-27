using System.Collections.Generic;

namespace StayFit.Services.Users
{
    public class LogWorkoutUserServiceModel
    {
        public string Name { get; set; }
        public bool HasWorkout { get; set; } = false;
        public bool IsWorkdayComplete { get; set; } = false;
        public Dictionary<string, string> DisplayExercises { get; init; } = new();
        public List<string> Exercises { get; init; }
        public List<int> Repetitions { get; init; }
        public List<int?> Weight { get; init; }
    }
}
