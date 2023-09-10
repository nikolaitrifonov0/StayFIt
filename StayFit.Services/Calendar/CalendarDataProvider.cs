using Microsoft.EntityFrameworkCore;
using StayFit.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StayFit.Services.Calendar
{
    public class CalendarDataProvider : ICalendarDataProvider
    {
        private readonly StayFitContext data;

        public CalendarDataProvider(StayFitContext data)
        {
            this.data = data;
        }

        public List<CalendarModel> GetData(string userId)
        {
            var result = data.UserExerciseLogs.Include(u => u.Exercise).Where(u => u.UserId == userId)
                .Select(u => new CalendarModel
                {
                    Id = u.Id,
                    Start = u.Date.ToString("yyyy-MM-dd"),
                    Title = u.Exercise.Name
                }).ToList();

            var filteredResult = FilterOutRepeatingElements(result);

            return filteredResult;
        }

        public CalendarDetailModel GetDetails(string exerciseName, DateTime date, string userId)
        {
            var logs = data.UserExerciseLogs.Include(u => u.Exercise)
                .Where(u => u.UserId == userId && u.Date == date && u.Exercise.Name == exerciseName)
                .Select(u => new CalendarExerciseDetailModel
                {
                    Id = u.Id,
                    Repetitions = u.Repetitions,
                    Set = u.SetNumber,
                    Weight = u.Weight
                }).OrderBy(u => u.Set).ToList();

            return new CalendarDetailModel
            {
                Title = exerciseName,
                Exercises = logs
            };
        }

        private List<CalendarModel> FilterOutRepeatingElements(List<CalendarModel> models)
        {
            var result = new List<CalendarModel>();
            foreach (var model in models)
            {
                if (!result.Any(u => u.Start == model.Start && u.Title == model.Title))
                {
                    result.Add(model);
                }
            }

            return result;
        }
    }
}
