using System.Collections.Generic;

namespace StayFit.Web.Models.Workouts
{
    public class DetailsWorkoutViewModel
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public List<DetailsWorkDayViewModel> WorkDays { get; set; }
        public int? CycleDays { get; init; }
    }
}
