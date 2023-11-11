using System;
using System.Collections.Generic;

namespace StayFit.Services.Calendar
{
    public class CalendarCreateModel
    {
        public string ExerciseId { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<CalendarExerciseDetailModel> Logs { get; set; }
    }
}
