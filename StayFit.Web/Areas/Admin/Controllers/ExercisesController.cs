using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayFit.Services.Exercises;
using static StayFit.Web.Areas.Admin.AdminConstants;

namespace StayFit.Web.Areas.Admin.Controllers
{
    [Area(AreaName)]
    [Authorize(Roles = AdministratorRoleName)]
    public class ExercisesController : Controller
    {
        private readonly IExerciseServices exercises;

        public ExercisesController(IExerciseServices exercises) => this.exercises = exercises;

        public IActionResult All() => View(this.exercises.All(publicOnly: false));

        public IActionResult Show(string id)
        {
            this.exercises.Show(id);

            return RedirectToAction("All");
        }
        public IActionResult Hide(string id)
        {
            this.exercises.Hide(id);

            return RedirectToAction("All");
        }
    }
}
