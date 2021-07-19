using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayFit.Data;
using StayFit.Data.Models.Enums.Workout;
using StayFit.Web.Infrastructure;
using StayFit.Web.Models;
using StayFit.Web.Models.Home;
using System;
using System.Diagnostics;
using System.Linq;

namespace StayFit.Web.Controllers
{
    public class HomeController : Controller
    {
        private StayFitContext data;

        public HomeController(StayFitContext data) => this.data = data;
        

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Log()
        {
            //UpdateWorkDays();            

            var model = new LogWorkoutHomeViewModel();

            if (this.data.Workouts.Any(w => w.Users.Any(u => u.Id == this.User.GetId())))
            {
                model.HasWorkout = true;
                var exercises = this.data.Workouts
                    .Where(w => w.Users.Any(u => u.Id == this.User.GetId()))
                    .Select(w => w.WorkDays.Select(wd => new {
                        wd.Id,
                        wd.NextWorkout,
                        Exercises = wd.Exercises.Select(e => new { e.Id, e.Name })
                    })
                    .Where(wd => wd.NextWorkout == DateTime.UtcNow)
                    .FirstOrDefault())
                    .FirstOrDefault();

                foreach (var exercise in exercises.Exercises)
                {
                    model.Exercises[exercise.Id] = exercise.Name;
                }
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>  View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        private void UpdateWorkDays()
        {
            var workDays = this.data.Workouts.FirstOrDefault(w => w.Users.Any(u => u.Id == this.User.GetId()))
                    .WorkDays
                    .OrderBy(wd => wd.NextWorkout);
            var workoutType = this.data.Workouts.FirstOrDefault(w => w.Users.Any(u => u.Id == this.User.GetId())).WorkoutCycleType;
            var cycleDays = this.data.Workouts.FirstOrDefault(w => w.Users.Any(u => u.Id == this.User.GetId())).CycleDays;

            if (workDays.Last().NextWorkout.DayOfYear < DateTime.UtcNow.DayOfYear)
            {
                if (workoutType == WorkoutCycleType.Weekly)
                {
                    foreach (var workDay in workDays)
                    {
                        workDay.NextWorkout.AddDays(7);
                    }
                }
                else if (workoutType == WorkoutCycleType.EveryNDays)
                {
                    foreach (var workDay in workDays)
                    {
                        workDay.NextWorkout.AddDays(cycleDays.Value);
                    }
                }
            }
        }
    }
}
