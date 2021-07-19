using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayFit.Data;
using StayFit.Data.Models;
using StayFit.Data.Models.Enums.Workout;
using StayFit.Web.Infrastructure;
using StayFit.Web.Models.Workouts;
using System;
using System.Collections.Generic;
using System.Linq;
using static StayFit.Data.DataConstants;

namespace StayFit.Web.Controllers
{
    public class WorkoutsController : Controller
    {
        private StayFitContext data;

        public WorkoutsController(StayFitContext data)
        {
            this.data = data;
        }

        public IActionResult All() => View(new AllWorkoutsViewModel
        {
            Workouts = this.SelectWorkouts()
        });        

        [Authorize]
        public IActionResult Add() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddWorkoutFormModel workout)
        {
            var exercisesToDays = ParseExercisesToDays(workout);

            if (workout.Exercises == null || exercisesToDays.Count == 0)
            {
                this.ModelState.AddModelError(nameof(workout.Exercises), "You must add some exercises.");
            }

            if (!this.ModelState.IsValid)
            {
                return View();
            }            

            var toAdd = new Workout
            {
                Name = workout.Name,
                Description = workout.Description,
                CycleDays = workout.CycleDays,
                WorkoutCycleType = workout.WorkoutCycleType == 0 ?
                WorkoutCycleType.Weekly : WorkoutCycleType.EveryNDays,
                CreatorId = this.User.GetId()
            };

            foreach (var ed in exercisesToDays)
            {
                var workDay = new WorkDay
                {
                    Workout = toAdd,
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
                        nextWorkout = toAdd.WorkDays.Last().NextWorkout.AddDays((double)toAdd.CycleDays);
                    }
                }

                workDay.NextWorkout = nextWorkout;

                toAdd.WorkDays.Add(workDay);
            }

            this.data.Workouts.Add(toAdd);
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Details(string id)
        {
            if (!this.data.Workouts.Any(w => w.Id == id))
            {
                return NotFound();
            }

            var workout = this.data.Workouts
                .Where(w => w.Id == id)
                .Select(w => new 
                {
                    Id = w.Id,
                    Name = w.Name,
                    Description = w.Description,
                    CycleType = w.WorkoutCycleType,
                    CycleDays = w.CycleDays,
                    WorkDays = w.WorkDays.Select(wd => new 
                        { 
                            Exercises = wd.Exercises.Select(e => e.Name).ToList(),
                            Day = wd.NextWorkout
                        }).ToList()
                }).FirstOrDefault();

            var model = new DetailsWorkoutViewModel
            {
                Id = workout.Id,
                Name = workout.Name,
                Description = workout.Description,
                CycleDays = workout.CycleDays
            };

            if (workout.CycleType == WorkoutCycleType.Weekly)
            {
                foreach (var day in workout.WorkDays)
                {
                    var dayName = day.Day.DayOfWeek.ToString();

                    model.ExercisesToDays[dayName] = day.Exercises;
                }
            }
            else if (workout.CycleType == WorkoutCycleType.EveryNDays)
            {
                for (int i = 0; i < workout.WorkDays.Count; i++)
                {
                    var dayName = $"Day {i + 1}";
                    model.ExercisesToDays[dayName] = workout.WorkDays[i].Exercises;
                }
            }

            return View(model);
        }

        private static Dictionary<string, List<string>> ParseExercisesToDays(AddWorkoutFormModel workout)
        {
            var result = new Dictionary<string, List<string>>();

            foreach (var exercise in workout.Exercises)
            {
                var splittedString = exercise.Split(" - ", StringSplitOptions.RemoveEmptyEntries);
                (string exercise, string day) ed = (splittedString[0], splittedString[1]);

                if (workout.WorkoutCycleType == (int)WorkoutCycleType.Weekly && DaysOfWeek.Any(d => d == ed.day))
                {
                    if (!result.ContainsKey(ed.day))
                    {
                        result[ed.day] = new List<string>();
                    }

                    result[ed.day].Add(ed.exercise);
                }
                else if (workout.WorkoutCycleType == (int)WorkoutCycleType.EveryNDays && !DaysOfWeek.Any(d => d == ed.day))
                {
                    if (!result.ContainsKey(ed.day))
                    {
                        result[ed.day] = new List<string>();
                    }
                    result[ed.day].Add(ed.exercise);
                }
            }

            return result;
        }

        private IEnumerable<WorkoutAllViewModel> SelectWorkouts()=>        
            this.data.Workouts.Select(w => new WorkoutAllViewModel
            {
                Id = w.Id,
                Name = w.Name,
                Description = w.Description,
                Creator = w.Creator.UserName,
                TotalWorkDays = w.WorkDays.Count
            }).ToList();
        
    }
}
