using StayFit.Data;
using System.Collections.Generic;
using System.Linq;

namespace StayFit.Services.Calendar
{
    public class CalendarUpdater : ICalendarUpdater
    {
        private readonly StayFitContext data;

        public CalendarUpdater(StayFitContext data)
        {
            this.data = data;
        }

        public void UpdateLogs(IEnumerable<CalendarExerciseDetailModel> logs)
        {
            var ids = logs.Select(l => l.Id);
            var logsToUpdate = this.data.UserExerciseLogs
                .Where(l => ids.Contains(l.Id)).ToList();

            foreach (var log in logsToUpdate)
            {
                var model = logs.First(l => l.Id == log.Id);

                log.Repetitions = model.Repetitions;
                log.Weight = model.Weight;
            }

            this.data.SaveChanges();
        }
    }
}
