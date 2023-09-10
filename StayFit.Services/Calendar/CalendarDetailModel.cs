using System.Collections.Generic;

namespace StayFit.Services.Calendar
{
    public class CalendarDetailModel
    {
        public string Title { get; set; }
        public List<CalendarExerciseDetailModel> Exercises { get; init; } 
    }
}
