using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StayFit.Data;
using StayFit.Data.Models;
using StayFit.Services.Statistics;
using StayFit.Test.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StayFit.Test.ServiceTests
{
    public class StatisticsServicesTest
    {
        [Fact]
        public void GetAllShouldReturnCorrectValues()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var statistics = new StatisticsServices(data);

            var user = new ApplicationUser();
            data.Users.Add(user);

            var workout = WorkoutProvider.TwoWorkouts().First();
            workout.Users.Add(user);

            var exercise = new Exercise
            {
                Name = "Deadlift",
                Description = "For the back",
                IsPublic = true,                
            };
            data.Exercises.Add(exercise);

            var workday = workout.WorkDays.ToList()[0];
            workday.Exercises.Add(exercise);


            var logs = new List<UserExerciseLog>
            {
                new UserExerciseLog
                {
                    Date = DateTime.Today,
                    Weight = 100,
                    Repetitions = 10,
                    SetNumber = 1,
                    UserId = user.Id,
                    Exercise = exercise
                },
                new UserExerciseLog
                {
                    Date = DateTime.Today.AddDays(5),
                    Weight = 105,
                    Repetitions = 10,
                    SetNumber = 1,
                    UserId = user.Id,
                    Exercise = exercise
                }
            };
            data.UserExerciseLogs.AddRange(logs);

            workday.UserExerciseLogs.Add(logs[0]);
            workday.UserExerciseLogs.Add(logs[1]);

            data.SaveChanges();

            var statistic = statistics.GetAll(user.Id);

            Assert.Single(statistic.UserMaxWeights);
            Assert.Single(statistic.UserMaxScores);

            Assert.True(statistic.UserMaxWeights.ContainsKey(exercise.Name));
            Assert.True(statistic.UserMaxScores.ContainsKey(exercise.Name));

            Assert.Equal(105, statistic.UserMaxWeights[exercise.Name].First().Weight);
            Assert.Equal(1050, statistic.UserMaxScores[exercise.Name].First().Score);
        }

        [Fact]
        public void GetAllShouldReturnEmptyCollectionsWithNonexistantUser()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var statistics = new StatisticsServices(data);           

            var statistic = statistics.GetAll("0");

            Assert.Empty(statistic.UserMaxWeights);
            Assert.Empty(statistic.UserMaxScores);
        }
    }

    
}
