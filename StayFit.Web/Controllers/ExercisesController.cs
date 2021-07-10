using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StayFit.Web.Controllers
{
    public class ExercisesController : Controller
    {
        public IActionResult Add() => View();
    }
}
