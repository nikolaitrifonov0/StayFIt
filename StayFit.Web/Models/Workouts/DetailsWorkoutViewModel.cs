using StayFit.Data.Models.Enums.Workout;
using System.Collections.Generic;

namespace StayFit.Web.Models.Workouts
{
    public class DetailsWorkoutViewModel
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public Dictionary<string, IEnumerable<string>> ExercisesToDays { get; init; } = new();
        public int? CycleDays { get; init; }
    }
}
