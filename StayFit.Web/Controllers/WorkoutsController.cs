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
            return RedirectToAction();
        }
    }
}
