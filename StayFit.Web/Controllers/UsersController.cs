using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayFit.Services.Exercises;
using StayFit.Services.Users;
using StayFit.Web.Infrastructure;

using static StayFit.Data.DataConstants;

namespace StayFit.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserServices users;

        public UsersController(IUserServices users)
        {
            this.users = users;
        }

        [Authorize]
        public IActionResult Log() => View(this.users.PrepareForView(this.User.GetId()));

        [Authorize]
        public IActionResult MoveWorkoutToToday()
        {
            users.MoveWorkoutToToday(this.User.GetId());
            return RedirectToAction("Log");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Log(LogWorkoutForUserServiceModel workout)
        {
            string userId = this.User.GetId();

            this.ModelState.Remove(nameof(workout.Repetitions));
            if (workout.ExerciseIds == null || workout.ExerciseIds.Count > workout.Repetitions.Count)
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

            if (!this.ModelState.IsValid)
            {               
                return View(this.users.PrepareForView(this.User.GetId()));
            }

            users.Log(workout, userId);

            return RedirectToAction();
        }
    }
}
