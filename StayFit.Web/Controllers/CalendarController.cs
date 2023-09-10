using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayFit.Services.Calendar;
using StayFit.Web.Infrastructure;
using System;
using System.Collections.Generic;

namespace StayFit.Web.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ICalendarDataProvider calendarDataProvider;
        private readonly ICalendarUpdater calendarUpdater;
        public CalendarController(ICalendarDataProvider calendarDataProvider, ICalendarUpdater calendarUpdater)
        {
            this.calendarDataProvider = calendarDataProvider;
            this.calendarUpdater = calendarUpdater;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetData()
        {
            var data = calendarDataProvider.GetData(this.User.GetId());

            return Ok(data);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetDetails(string exerciseName, DateTime date)
        {
            var data = calendarDataProvider.GetDetails(exerciseName, date, this.User.GetId());

            return Ok(data);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult UpdateDetails([FromBody]IEnumerable<CalendarExerciseDetailModel> logs)
        {
            if (!ModelState.IsValid)
            { 
                return View("Users/Log");
            }

            calendarUpdater.UpdateLogs(logs);

            return Ok();
        }
    }
}
