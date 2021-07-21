using System.Collections.Generic;

namespace StayFit.Services.Workouts
{
    public class AllWorkoutsServiceModel
    {
        public IEnumerable<WorkoutAllServiceModel> Workouts { get; init; }
    }
}
