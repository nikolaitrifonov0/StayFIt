using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StayFit.Web.Models.Workouts
{
    public class AllWorkoutsViewModel
    {
        public IEnumerable<WorkoutViewModel> Workouts { get; init; }
    }
}
