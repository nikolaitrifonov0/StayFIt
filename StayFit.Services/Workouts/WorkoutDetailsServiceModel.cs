using StayFit.Data.Models.Enums.Workout;
using System.Collections.Generic;

namespace StayFit.Services.Workouts
{
    public class WorkoutDetailsServiceModel
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public List<DetailsWorkDayServiceModel> WorkDays { get; set; }
        public int? CycleDays { get; init; }
        public WorkoutCycleType CycleType { get; init; }
    }
}
