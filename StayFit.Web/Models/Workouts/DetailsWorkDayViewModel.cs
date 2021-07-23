using System.Collections.Generic;

namespace StayFit.Web.Models.Workouts
{
    public class DetailsWorkDayViewModel
    {
        public string Day { get; set; }
        public Dictionary<string, string> Exercises { get; set; }
    }
}

