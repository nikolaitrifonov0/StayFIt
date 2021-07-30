using StayFit.Data;
using StayFit.Data.Models;
using StayFit.Data.Models.Enums.Workout;
using System;
using System.Linq;

namespace StayFit.Services.Users
{
    public class UserServices : IUserServices
    {
        private readonly StayFitContext data;

        public UserServices(StayFitContext data)
        {
            this.data = data;
        }

        public void Log(LogWorkoutUserServiceModel workout, string userId)
        {
            for (int i = 0; i < workout.Exercises.Count(); i++)
            {
                var log = new UserExerciseLog
                {
                    ExerciseId = workout.Exercises[i],
                    SetNumber = i + 1,
                    Weight = workout.Weight.Count > 0 ? workout.Weight[i] : null,
                    Repetitions = workout.Repetitions[i],
                    UserId = userId,
                    WorkDayId = this.data.WorkDays
                    .Where(wd => wd.Workout.Users.Any(u => u.Id == userId)
                        && wd.NextWorkout.DayOfYear == DateTime.Today.DayOfYear)
                    .Select(wd => wd.Id)
                    .First(),
                    Date = DateTime.Today,
                };

                this.data.UserExerciseLogs.Add(log);
                this.data.SaveChanges();
            }
        }

        public LogWorkoutUserServiceModel PrepareForView(string userId)
        {
            UpdateWorkDays(userId);

            var model = new LogWorkoutUserServiceModel();

            if (this.data.Workouts.Any(w => w.Users.Any(u => u.Id == userId)))
            {
                model.HasWorkout = true;
                model.IsWorkdayComplete = this.data.UserExerciseLogs
                    .Any(uel => uel.UserId == userId && uel.Date == DateTime.Today);
                model.Name = this.GetUserWorkout(userId)
                    .Select(w => w.Name)
                    .FirstOrDefault();

                var exercises = this.GetUserWorkout(userId)
                    .Select(w =>
                    w.WorkDays.Select(wd => new
                    {
                        wd.Id,
                        wd.NextWorkout,
                        Exercises = wd.Exercises.Select(e => new { e.Id, e.Name }).ToList()
                    })
                    .Where(wd => wd.NextWorkout.DayOfYear == DateTime.Today.DayOfYear)
                    .FirstOrDefault())
                    .FirstOrDefault();

                if (exercises != null)
                {
                    foreach (var exercise in exercises.Exercises)
                    {
                        model.DisplayExercises[exercise.Id] = exercise.Name;
                    }
                }
            }

            return model;
        }

        private void UpdateWorkDays(string userId)
        {
            var workdays = this.data.WorkDays
                .Where(wd => wd.Workout.Users.Any(u => u.Id == userId));

            if (!workdays.Any(wd => wd.NextWorkout.DayOfYear >= DateTime.Today.DayOfYear))
            {
                var workoutCycleType = this.GetUserWorkout(userId)
                .First().WorkoutCycleType;

                while (!workdays.Any(wd => wd.NextWorkout.DayOfYear >= DateTime.Today.DayOfYear))
                {
                    if (workoutCycleType == WorkoutCycleType.Weekly)
                    {
                        foreach (var workday in workdays)
                        {
                            workday.NextWorkout = workday.NextWorkout.AddDays(7);
                        }
                    }
                    else if (workoutCycleType == WorkoutCycleType.EveryNDays)
                    {
                        var workoutCycleDays = this.GetUserWorkout(userId)
                            .First().CycleDays;
                        foreach (var workday in workdays)
                        {
                            workday.NextWorkout = workday.NextWorkout.AddDays(workoutCycleDays.Value);
                        }
                    }

                    this.data.SaveChanges();
                }
            }
        }

        private IQueryable<Workout> GetUserWorkout(string userId)
            => this.data.Workouts
                .Where(w => w.Users.Any(u => u.Id == userId));
    }
}
