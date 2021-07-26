using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StayFit.Web.Models.Users
{
    public class LogWorkoutFormModel
    {
        public string Name { get; set; }
        public bool HasWorkout { get; set; } = false;
        public bool IsWorkdayComplete { get; set; } = false;
        public Dictionary<string, string> DisplayExercises { get; init; } = new();
        [Required]
        public List<string> Exercises { get; init; }
        public List<int> Repetitions { get; init; }
        [Required]
        public List<int?> Weight { get; init; }
    }
}
