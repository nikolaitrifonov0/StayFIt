using System.Collections.Generic;

namespace StayFit.Services.Calendar
{
    public interface ICalendarDeleter
    {
        void Delete(IEnumerable<string> logs);
    }
}
