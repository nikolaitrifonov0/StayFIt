using Microsoft.EntityFrameworkCore;
using StayFit.Data;
using StayFit.Data.Models;
using StayFit.Data.Models.Enums.Workout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StayFit.Services.Workouts
{
    public class WorkoutServices : IWorkoutServices
    {
        private readonly StayFitContext data;

        public WorkoutServices(StayFitContext data) => this.data = data;

        public void Add(string name, string description, int? cycleDays, 
            int workoutCycleType, string creatorId, Dictionary<string, List<string>> exercisesToDays)
        {
            var workout = new Workout
            {
                Name = name,
                Description = description,
                CycleDays = cycleDays,
                WorkoutCycleType = workoutCycleType == 0 ?
                WorkoutCycleType.Weekly : WorkoutCycleType.EveryNDays,
                CreatorId = creatorId
            };

            foreach (var ed in exercisesToDays)
            {
                var workDay = new WorkDay
                {
                    Workout = workout,
                    Exercises = this.data.Exercises.Where(e => ed.Value.Contains(e.Id)).ToHashSet()
                };

                DateTime nextWorkout;

                if (Enum.IsDefined(typeof(DayOfWeek), ed.Key))
                {
                    var dayOfWeek = Enum.Parse(typeof(DayOfWeek), ed.Key);

                    var tomorrow = DateTime.UtcNow.AddDays(1);

                    var daysUntilNextWorkout = ((int)dayOfWeek - (int)tomorrow.DayOfWeek + 7) % 7;
                    nextWorkout = tomorrow.AddDays(daysUntilNextWorkout);
                }
                else
                {
                    if (exercisesToDays.Keys.First() == ed.Key)
                    {
                        nextWorkout = DateTime.UtcNow.AddDays(1);
                    }
                    else
                    {
                        nextWorkout = workout.WorkDays.Last().NextWorkout.AddDays((double)workout.CycleDays);
                    }
                }

                workDay.NextWorkout = nextWorkout;

                workout.WorkDays.Add(workDay);
            }

            this.data.Workouts.Add(workout);
            this.data.SaveChanges();
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

        public WorkoutEditServiceModel Details(string id)
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
                .Select(w => new WorkoutEditServiceModel
                {
                    Id = w.Id,
                    Name = w.Name,
                    Description = w.Description,
                    CycleType = w.WorkoutCycleType,
                    CycleDays = w.CycleDays,
                    CreatorId = w.CreatorId,
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

        public void Edit(string id, string name, string description, int? cycleDays, 
            int workoutCycleType, Dictionary<string, List<string>> exercisesToDays)
        {
            var workout = this.data.Workouts.Find(id);
            workout.Name = name;
            workout.Description = description;
            workout.CycleDays = cycleDays;
            workout.WorkoutCycleType = workoutCycleType == 0 ?
                WorkoutCycleType.Weekly : WorkoutCycleType.EveryNDays;


            if (exercisesToDays != null)
            {
                var workDays = this.data.WorkDays
                    .Where(wd => wd.WorkoutId == workout.Id).ToList();

                this.data.WorkDays.RemoveRange(workDays);
               

                foreach (var ed in exercisesToDays)
                {
                    var workDay = new WorkDay
                    {
                        Workout = workout,
                        Exercises = this.data.Exercises.Where(e => ed.Value.Contains(e.Id)).ToHashSet()
                    };

                    DateTime nextWorkout;

                    if (Enum.IsDefined(typeof(DayOfWeek), ed.Key))
                    {
                        var dayOfWeek = Enum.Parse(typeof(DayOfWeek), ed.Key);

                        var tomorrow = DateTime.UtcNow.AddDays(1);

                        var daysUntilNextWorkout = ((int)dayOfWeek - (int)tomorrow.DayOfWeek + 7) % 7;
                        
                        nextWorkout = tomorrow.AddDays(daysUntilNextWorkout);
                    }
                    else
                    {
                        if (exercisesToDays.Keys.First() == ed.Key)
                        {
                            nextWorkout = DateTime.UtcNow.AddDays(1);
                        }
                        else
                        {
                            nextWorkout = workout.WorkDays.Last().NextWorkout.AddDays((double)workout.CycleDays);
                        }
                    }

                    workDay.NextWorkout = nextWorkout;

                    workout.WorkDays.Add(workDay);
                }                
            }

            this.data.SaveChanges();
        }

        public EditWorkoutsServiceModel EditDetails(string id) 
            => this.data.Workouts.Where(w => w.Id == id)
                .Select(w => new EditWorkoutsServiceModel
                {
                    Name = w.Name,
                    Description = w.Description,
                    CycleDays = w.CycleDays,
                    WorkoutCycleType = (int)w.WorkoutCycleType
                }).FirstOrDefault();

        public bool IsCreator(string workoutId, string userId)
            => this.data.Workouts.Find(workoutId).CreatorId == userId;
    }
}
