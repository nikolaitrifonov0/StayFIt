using Microsoft.EntityFrameworkCore;
using StayFit.Data;
using StayFit.Data.Models;
using StayFit.Data.Models.Enums.Workout;
using System;
using System.Collections.Generic;
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

        public void Log(LogWorkoutForUserServiceModel workout, string userId)
        {
            if (!this.data.WorkDays
                .Any(wd => wd.Workout.Users.Any(u => u.Id == userId && u.NextWorkout.HasValue && u.NextWorkout.Value.DayOfYear == DateTime.Today.DayOfYear)))
            {
                return;
            }

            Workout workoutEntity = null;
            for (int i = 0; i < workout.ExerciseIds.Count(); i++)
            {
                var log = new UserExerciseLog
                {
                    ExerciseId = workout.ExerciseIds[i],
                    SetNumber = i + 1,
                    Weight = workout.Weight.Count > 0 ? workout.Weight[i] : null,
                    Repetitions = workout.Repetitions[i],
                    UserId = userId,
                    WorkDayId = this.data.Users.Where(u => userId == u.Id).First().NextWorkDayId,
                    Date = DateTime.Today,
                };

                this.data.UserExerciseLogs.Add(log);
                this.data.SaveChanges();
                workoutEntity = this.data.Workouts.Where(w => w.WorkDays.Any(wd => wd.Id == log.WorkDayId)).First();
            }

            var user = this.data.Users.Find(userId);
            this.AssignNextWorkDay(user, workoutEntity);
        }

        public void MoveWorkoutToToday(string userId)
        {
            var user = this.data.Users.Find(userId);
            user.NextWorkout = DateTime.Today;
            this.data.SaveChanges();
        }

        public LogWorkoutForUserServiceModel PrepareForView(string userId)
        {
            var model = new LogWorkoutForUserServiceModel();
            var user = this.data.Users.Find(userId);

            if (!this.data.Workouts.Any(w => w.Users.Any(u => u.Id == userId)))
            {
                return model;
            }

            if (user.NextWorkout.HasValue && user.NextWorkout.Value.DayOfYear < DateTime.Now.DayOfYear)
            {
                this.AssignNextWorkDay(user, this.GetUserWorkout(userId).First());
            }

            model.HasWorkout = true;
            model.IsWorkdayComplete = this.data.UserExerciseLogs
                .Any(uel => uel.UserId == userId && uel.Date == DateTime.Today);
            model.Name = this.GetUserWorkout(userId)
                .Select(w => w.Name)
                .FirstOrDefault();

            var exercises = this.GetUserWorkout(userId)
                .Select(w =>
                w.WorkDays.Where(wd => user.NextWorkDayId == wd.Id && user.NextWorkout.HasValue && user.NextWorkout.Value.DayOfYear == DateTime.Today.DayOfYear)
                .Select(wd => new
                {
                    wd.Id,
                    Exercises = wd.Exercises.Select(e => new { e.Id, e.Name }).ToList()
                })
                .FirstOrDefault())
                .FirstOrDefault();

            if (exercises != null)
            {
                foreach (var exercise in exercises.Exercises)
                {
                    model.DisplayExercises[exercise.Id] = exercise.Name;
                }
            }

            var workoutId = this.GetUserWorkout(userId).FirstOrDefault().Id;
            var logs = this.data.UserExerciseLogs
                .Select(l => new {
                    l.UserId,
                    l.Date,
                    ExerciseName = l.Exercise.Name,
                    l.SetNumber,
                    l.Repetitions,
                    l.Weight,
                    WorkoutId = l.WorkDay.Workout.Id
                })
                .Where(l => l.UserId == userId && workoutId == l.WorkoutId)
                .OrderByDescending(l => l.Date)
                .ToList();

            if (logs.Count != 0)
            {
                var lastLogsDate = logs.First().Date;
                logs = logs.Where(l => l.Date == lastLogsDate).ToList();

                foreach (var log in logs)
                {
                    if (!model.LastWorkoutLogs.ContainsKey(log.ExerciseName))
                    {
                        model.LastWorkoutLogs[log.ExerciseName] = new List<LastWorkoutLogServiceModel>();
                    }
                    if (model.LastWorkoutLogs[log.ExerciseName].Count < 3)
                    {
                        model.LastWorkoutLogs[log.ExerciseName].Add(new LastWorkoutLogServiceModel
                        {
                            Set = log.SetNumber,
                            Repetitions = log.Repetitions,
                            Weight = log.Weight
                        });
                    }
                }
            }

            model.NextWorkout = user.NextWorkout;

            return model;
        }

        public void AssignNextWorkDay(ApplicationUser user, Workout workout, bool isNewWorkout = false)
        {
            var workDays = data.WorkDays.Where(wd => wd.WorkoutId == workout.Id).ToList();
            var lastWorkday = data.UserExerciseLogs.Include(uel => uel.WorkDay).Where(uel => uel.UserId == user.Id).OrderByDescending(uel => uel.Date).FirstOrDefault()?.WorkDay;
            if (workout.WorkoutCycleType == WorkoutCycleType.Weekly)
            {
                bool isWorkdayAssigned = false;
                DateTime date = DateTime.Today.AddDays(1);
                while (isWorkdayAssigned == false)
                {
                    if (workDays.Any(w => w.DayOfWeek == date.DayOfWeek && (lastWorkday == null || lastWorkday.Id != w.Id)))
                    {
                        user.NextWorkDayId = workDays.First(w => w.DayOfWeek == date.DayOfWeek).Id;
                        user.NextWorkout = date;
                        isWorkdayAssigned = true;
                    }
                    else
                    {
                        date = date.AddDays(1);
                    }
                }
            }
            else
            {
                if (user.NextWorkout == null || isNewWorkout)
                {
                    user.NextWorkDayId = workDays[0].Id;
                    user.NextWorkout = DateTime.Today.AddDays(1);
                }
                else
                {
                    var nextWorkday = workDays.FirstOrDefault(w => w.Id == user.NextWorkDayId + 1);

                    if (nextWorkday == null)
                    {
                        nextWorkday = workDays[0];
                    }
                    user.NextWorkDayId = workDays[0].Id;


                    while (user.NextWorkout.Value < DateTime.Today)
                    {
                        user.NextWorkout = user.NextWorkout.Value.AddDays(workout.CycleDays.Value);
                    }
                }
            }

            this.data.SaveChanges();
        }

        private IQueryable<Workout> GetUserWorkout(string userId)
            => this.data.Workouts
                .Where(w => w.Users.Any(u => u.Id == userId));
    }
}
