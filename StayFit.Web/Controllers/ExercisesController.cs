using Microsoft.AspNetCore.Mvc;

namespace StayFit.Web.Controllers
{
    public class ExercisesController : Controller
    {
        public IActionResult Add() => View();
    }
}
