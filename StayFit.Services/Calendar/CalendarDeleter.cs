using StayFit.Data;
using System.Collections.Generic;
using System.Linq;

namespace StayFit.Services.Calendar
{
    public class CalendarDeleter : ICalendarDeleter
    {
        private readonly StayFitContext data;
        public CalendarDeleter(StayFitContext data)
        {
            this.data = data;
        }

        public void Delete(IEnumerable<string> logs)
        {
            var toDelete = data.UserExerciseLogs.Where(l => logs.Contains(l.Id)).ToList();
            data.UserExerciseLogs.RemoveRange(toDelete);

            data.SaveChanges();
        }
    }
}
