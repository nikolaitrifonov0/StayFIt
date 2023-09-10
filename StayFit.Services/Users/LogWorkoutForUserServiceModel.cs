using System;
using System.Collections.Generic;

namespace StayFit.Services.Users
{
    public class LogWorkoutForUserServiceModel
    {
        public string Name { get; set; }
        public DateTime? NextWorkout { get; set; }
        public bool HasWorkout { get; set; } = false;
        public bool IsWorkdayComplete { get; set; } = false;
        public Dictionary<string, string> DisplayExercises { get; init; } = new();
        public List<string> ExerciseIds { get; init; }
        public List<int> Repetitions { get; init; }
        public List<int?> Weight { get; init; }
        public Dictionary<string, List<LastWorkoutLogServiceModel>> LastWorkoutLogs { get; init; } = new();
    }
}
