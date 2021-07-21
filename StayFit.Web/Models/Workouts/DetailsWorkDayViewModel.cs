using System;
using System.Collections.Generic;

namespace StayFit.Web.Models.Workouts
{
    public class DetailsWorkDayViewModel
    {
        public string Day { get; set; }
        public IEnumerable<string> Exercises { get; set; }
        public DateTime NextWorkout { get; init; }
    }
}

