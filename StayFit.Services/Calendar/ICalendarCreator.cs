using System;
using System.Collections.Generic;

namespace StayFit.Services.Calendar
{
    public interface ICalendarCreator
    {
        void Create(CalendarCreateModel createModel, string userId);
    }
}
