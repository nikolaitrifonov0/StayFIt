using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayFit.Data;
using StayFit.Data.Models;
using StayFit.Data.Models.Enums.Workout;
using StayFit.Web.Infrastructure;
using StayFit.Web.Models.Users;
using System;
using System.Linq;

using static StayFit.Data.DataConstants;

namespace StayFit.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly StayFitContext data;

        public UsersController(StayFitContext data) => this.data = data;

        [Authorize]
        public IActionResult Log() 
        {
            UpdateWorkDays();            

            var model = new LogWorkoutFormModel();

            if (this.data.Workouts.Any(w => w.Users.Any(u => u.Id == this.User.GetId())))
            {
                model.HasWorkout = true;
                model.IsWorkdayComplete = this.data.UserExerciseLogs
                    .Any(uel => uel.UserId == this.User.GetId() && uel.Date == DateTime.Today);
                model.Name = this.data.Workouts
                    .Where(w => w.Users.Any(u => u.Id == this.User.GetId()))
                    .Select(w => w.Name)
                    .FirstOrDefault();                

                var exercises = this.data.Workouts
                    .Where(w => w.Users.Any(u => u.Id == this.User.GetId()))
                    .Select(w => 
                    w.WorkDays.Select(wd => new {
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

            return View(model);
        }

        private void UpdateWorkDays()
        {
            var workdays = this.data.WorkDays
                .Where(wd => wd.Workout.Users.Any(u => u.Id == this.User.GetId()));

            if (!workdays.Any(wd => wd.NextWorkout.DayOfYear >= DateTime.Today.DayOfYear))
            {
                var workoutCycleType = this.data.Workouts
                .Where(w => w.Users.Any(u => u.Id == this.User.GetId()))
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
                        var workoutCycleDays = this.data.Workouts
                            .Where(w => w.Users.Any(u => u.Id == this.User.GetId()))
                            .First().CycleDays;
                        foreach (var workday in workdays)
                        {
                            workday.NextWorkout =  workday.NextWorkout.AddDays(workoutCycleDays.Value);
                        }
                    }

                    this.data.SaveChanges();
                }
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Log(LogWorkoutFormModel workout)
        {
            this.ModelState.Remove(nameof(workout.Repetitions));
            if (workout.Exercises == null || workout.Exercises.Count > workout.Repetitions.Count)
            {
                this.ModelState.AddModelError(nameof(workout.Repetitions), "You must not leave empty reps fields.");
            }

            foreach (var weight in workout.Weight)
            {
                if (weight != null && (weight > WeightMaxLenght || weight < WeightMinLenght))
                {
                    this.ModelState.AddModelError(nameof(workout.Weight), $"The weight should be between {WeightMinLenght} and {WeightMaxLenght}.");
                }
            }

            foreach (var exercise in workout.Exercises)
            {
                if (!this.data.WorkDays
                    .Where(wd => wd.Workout.Users.Any(u => u.Id == this.User.GetId())
                        && wd.NextWorkout.DayOfYear == DateTime.Today.DayOfYear)
                    .Select(wd => new { wd.Exercises })
                    .First()
                    .Exercises.Any(e => e.Id == exercise))
                {
                    this.ModelState.AddModelError(nameof(workout.Exercises), "This exercise is not in your workout.");
                }
            }

            if (!this.ModelState.IsValid)
            {
                var model = new LogWorkoutFormModel();

                model.HasWorkout = true;
                model.IsWorkdayComplete = this.data.UserExerciseLogs
                    .Any(uel => uel.UserId == this.User.GetId() && uel.Date == DateTime.Today);
                model.Name = this.data.Workouts
                    .Where(w => w.Users.Any(u => u.Id == this.User.GetId()))
                    .Select(w => w.Name)
                    .FirstOrDefault();

                var exercises = this.data.Workouts
                    .Where(w => w.Users.Any(u => u.Id == this.User.GetId()))
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

                return View(model);
            }

            string userId = this.User.GetId();

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

            return RedirectToAction();
        }
    }
}
