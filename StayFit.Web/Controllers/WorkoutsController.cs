using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayFit.Data.Models.Enums.Workout;
using StayFit.Services.Workouts;
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
        private readonly IWorkoutServices workouts;
        private readonly IMapper mapper;

        public WorkoutsController(IWorkoutServices workouts, IMapper mapper)
        {
            this.workouts = workouts;
            this.mapper = mapper;
        }

        public IActionResult All() => View(this.workouts.All());

        public IActionResult MyWorkouts() => View("All", this.workouts.MyWorkouts(this.User.GetId()));

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

            this.workouts.Add(workout.Name, workout.Description,
                workout.CycleDays, workout.WorkoutCycleType, this.User.GetId(), exercisesToDays);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Details(string id)
        {
            var workoutService = workouts.Details(id);

            if (workoutService == null)
            {
                return NotFound();
            }

            var workout = this.mapper.Map<DetailsWorkoutViewModel>(workoutService);
            workout.IsCreator = this.User.Identity.IsAuthenticated && workouts.IsCreator(workoutService.Id, this.User.GetId());
            workout.WorkDays = workoutService.WorkDays
                .Select(wd => this.mapper.Map<DetailsWorkDayViewModel>(wd))
                .ToList();

            return View(workout);
        }        

        [Authorize]
        public IActionResult Assign(string id)
        {
            workouts.Assign(this.User.GetId(), id);

            return RedirectToAction("Log", "Users");
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            if (!workouts.IsCreator(id, this.User.GetId()) && !this.User.IsAdmin())
            {
                return RedirectToAction("Add");
            }

            return View(workouts.EditDetails(id));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(string id, EditWorkoutsServiceModel workout)
        {
            if (!this.ModelState.IsValid)
            {
                return View();
            }

            var exercisesToDays = workout.Exercises != null ? ParseExercisesToDays(workout) : null;

            this.workouts.Edit(id,
                workout.Name,
                workout.Description,
                workout.CycleDays,
                workout.WorkoutCycleType,
                workout.ImageUrl,
                exercisesToDays);

            return Redirect($"/Workouts/{nameof(this.Details)}/{id}");
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
        private static Dictionary<string, List<string>> ParseExercisesToDays(EditWorkoutsServiceModel workout)
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
    }
}
