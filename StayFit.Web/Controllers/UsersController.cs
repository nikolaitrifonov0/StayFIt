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
        private readonly IExerciseServices exercises;

        public UsersController(IUserServices users, IExerciseServices exercises)
        {
            this.users = users;
            this.exercises = exercises;
        }

        [Authorize]
        public IActionResult Log() => View(users.PrepareForView(this.User.GetId()));

        [HttpPost]
        [Authorize]
        public IActionResult Log(LogWorkoutUserServiceModel workout)
        {
            string userId = this.User.GetId();

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
                if (!exercises.IsInWorkout(exercise, userId))
                {
                    this.ModelState.AddModelError(nameof(workout.Exercises), "This exercise is not in your workout.");
                }
            }

            if (!this.ModelState.IsValid)
            {               
                return View(users.PrepareForView(this.User.GetId()));
            }

            users.Add(workout, userId);

            return RedirectToAction();
        }
    }
}
