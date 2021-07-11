using Microsoft.AspNetCore.Mvc;

namespace StayFit.Web.Controllers
{
    public class WorkoutsController : Controller
    {
        public IActionResult Add() => View();
    }
}
