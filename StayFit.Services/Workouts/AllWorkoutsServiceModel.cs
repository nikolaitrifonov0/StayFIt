using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayFit.Services.Workouts
{
    public class AllWorkoutsServiceModel
    {
        public IEnumerable<WorkoutAllServiceModel> Workouts { get; init; }
    }
}
