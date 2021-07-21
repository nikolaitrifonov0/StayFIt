using System.Collections.Generic;

namespace StayFit.Web.Models.Workouts
{
    public class AllWorkoutsViewModel
    {
        public IEnumerable<WorkoutAllViewModel> Workouts { get; init; }
    }
}
