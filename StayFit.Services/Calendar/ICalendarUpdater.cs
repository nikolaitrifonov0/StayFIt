using System.Collections.Generic;

namespace StayFit.Services.Calendar
{
    public interface ICalendarUpdater
    {
        void UpdateLogs(IEnumerable<CalendarExerciseDetailModel> logs);
    }
}
