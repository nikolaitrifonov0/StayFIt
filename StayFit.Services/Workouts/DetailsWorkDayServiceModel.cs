using System;
using System.Collections.Generic;

namespace StayFit.Services.Workouts
{
    public class DetailsWorkDayServiceModel
    {
        public string Day { get; set; }
        public IEnumerable<string> Exercises { get; set; }
        public DateTime NextWorkout { get; init; }
    }
}
