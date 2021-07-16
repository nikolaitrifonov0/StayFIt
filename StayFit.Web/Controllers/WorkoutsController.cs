using Microsoft.AspNetCore.Mvc;
using StayFit.Web.Models.Workouts;

namespace StayFit.Web.Controllers
{
    public class WorkoutsController : Controller
    {
        public IActionResult Add() => View();
        [HttpPost]
        public IActionResult Add(AddWorkoutFormModel workout)
        {
            if (workout.Exercises == null)
            {
                this.ModelState.AddModelError(nameof(workout.Exercises), "You must add some exercises.");
            }

            if (!this.ModelState.IsValid)
            {
                return View();
            }

            return RedirectToAction();
        }
    }
}
