using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StayFit.Web.Models.Workouts
{
    public class AddWorkoutViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int WorkoutCycleType { get; set; }
        public int? CycleDays { get; set; }
    }
}
