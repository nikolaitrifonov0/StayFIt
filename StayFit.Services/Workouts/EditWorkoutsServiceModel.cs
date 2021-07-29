using System.Collections.Generic;

namespace StayFit.Services.Workouts
{
    public class EditWorkoutsServiceModel
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public int WorkoutCycleType { get; init; }
        public int? CycleDays { get; init; }
        public IEnumerable<string> Exercises { get; init; }
    }
}
