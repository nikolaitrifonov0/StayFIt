using System;
using System.Collections.Generic;

namespace StayFit.Services.Calendar
{
    public interface ICalendarDataProvider
    {
        List<CalendarModel> GetData(string userId);
        CalendarDetailModel GetDetails(string exerciseName, DateTime date, string userId);
    }
}
