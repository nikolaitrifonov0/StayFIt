using Microsoft.EntityFrameworkCore;
using StayFit.Data;
using StayFit.Data.Models.Enums.Workout;
using System.Linq;

namespace StayFit.Services.Workouts
{
    public class WorkoutServices : IWorkoutServices
    {
        private readonly StayFitContext data;

        public WorkoutServices(StayFitContext data)
        {
            this.data = data;
        }

        public AllWorkoutsServiceModel All() => new AllWorkoutsServiceModel
        {
            Workouts = this.data.Workouts.Select(w => new WorkoutAllServiceModel
            {
                Id = w.Id,
                Name = w.Name,
                Description = w.Description,
                Creator = w.Creator.UserName,
                TotalWorkDays = w.WorkDays.Count
            }).ToList()
        };

        public void Assign(string userId, string workoutId)
        {
            var user = this.data.Users.Where(u => u.Id == userId).FirstOrDefault();
            var workout = this.data.Workouts.Where(w => w.Id == workoutId).FirstOrDefault();

            if (workout == null || workout.Users.Any(u => u.Id == userId))
            {
                return;
            }

            workout.Users.Add(user);
            data.SaveChanges();
        }

        public WorkoutDetailsServiceModel Details(string id)
        {
            if (!this.data.Workouts.Any(w => w.Id == id))
            {
                return null;
            }

            var workout = this.data.Workouts
                .Include(w => w.WorkDays)
                .ThenInclude(w => w.Exercises)
                .AsEnumerable()
                .Where(w => w.Id == id)
                .Select(w => new WorkoutDetailsServiceModel
                {
                    Id = w.Id,
                    Name = w.Name,
                    Description = w.Description,
                    CycleType = w.WorkoutCycleType,
                    CycleDays = w.CycleDays,
                    WorkDays = w.WorkDays.Select(wd => new DetailsWorkDayServiceModel
                    {
                        Exercises = wd.Exercises
                        .Select(e => new { e.Id, e.Name }).ToDictionary(e => e.Id, e => e.Name),
                        NextWorkout = wd.NextWorkout
                    }).ToList()
                }).FirstOrDefault();

            if (workout.CycleType == WorkoutCycleType.Weekly)
            {
                foreach (var day in workout.WorkDays)
                {
                    var dayName = day.NextWorkout.DayOfWeek.ToString();

                    day.Day = dayName;
                }
                workout.WorkDays = workout.WorkDays.OrderBy(d => d.NextWorkout.DayOfWeek).ToList();
            }
            else if (workout.CycleType == WorkoutCycleType.EveryNDays)
            {
                for (int i = 0; i < workout.WorkDays.Count; i++)
                {
                    var dayName = $"Day {i + 1}";

                    workout.WorkDays[i].Day = dayName;
                }
            }

            return workout;
        }
    }
}
