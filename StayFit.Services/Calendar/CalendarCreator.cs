using StayFit.Data;
using StayFit.Data.Models;
using System;
using System.Collections.Generic;

namespace StayFit.Services.Calendar
{
    public class CalendarCreator : ICalendarCreator
    {
        private readonly StayFitContext data;

        public CalendarCreator(StayFitContext data)
        {
            this.data = data;
        }

        public void Create(CalendarCreateModel createModel, string userId)
        {
            foreach (var log in createModel.Logs)
            {
                var logToCreate = new UserExerciseLog
                {
                    Date = createModel.Date,
                    ExerciseId = createModel.ExerciseId,
                    Repetitions = log.Repetitions,
                    SetNumber = log.Set,
                    Weight = log.Weight,
                    UserId = userId
                };

                this.data.UserExerciseLogs.Add(logToCreate);
                this.data.SaveChanges();
            }
        }
    }
}
