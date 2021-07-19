using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StayFit.Web.Models.Workouts
{
    public class WorkoutViewModel
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string Creator { get; init; }
        public int TotalWorkDays { get; init; }
    }
}
