using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayFit.Data;
using StayFit.Web.Infrastructure;
using StayFit.Web.Models.Users;
using System;
using System.Linq;

namespace StayFit.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly StayFitContext data;

        public UsersController(StayFitContext data) => this.data = data;

        [Authorize]
        public IActionResult Log() 
        {
            //UpdateWorkDays();            

            var model = new LogWorkoutViewModel();

            if (this.data.Workouts.Any(w => w.Users.Any(u => u.Id == this.User.GetId())))
            {
                model.HasWorkout = true;
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

                foreach (var exercise in exercises.Exercises)
                {
                    model.Exercises[exercise.Id] = exercise.Name;
                }
            }

            return View(model);
        }
    }
}
