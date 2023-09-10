using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StayFit.Data;
using StayFit.Data.Models;
using StayFit.Services.Users;
using StayFit.Test.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StayFit.Test.ServiceTests
{
    public class UserServicesTest
    {
        [Fact]
        public void LogShouldBeAddedToDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var users = new UserServices(data);

            var user = new ApplicationUser();
            var workout = WorkoutProvider.TwoWorkouts().First();

            data.Users.Add(user);
            data.Workouts.Add(workout);
            workout.Users.Add(user);
            data.SaveChanges();

            var log = new LogWorkoutForUserServiceModel
            {
                ExerciseIds = new List<string>()
                {
                    "Squat",
                    "Pullup"
                },
                Weight = new List<int?>()
                {
                    100,
                    null
                },
                Repetitions = new List<int>()
                {
                    10,
                    20
                }
            };

            users.Log(log, user.Id);

            var result = data.UserExerciseLogs.Select(r => new { r.ExerciseId, r.Weight, r.Repetitions }).ToList();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.ExerciseId == "Squat" && r.Weight == 100 && r.Repetitions == 10);
            Assert.Contains(result, r => r.ExerciseId == "Pullup" && r.Weight == null && r.Repetitions == 20);
        }

        [Fact]
        public void LogWithInvalidShouldNotBeAddedToDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var users = new UserServices(data);

            var log = new LogWorkoutForUserServiceModel
            {
                ExerciseIds = new List<string>()
                {
                    "Squat",
                    "Pullup"
                },
                Weight = new List<int?>()
                {
                    100,
                    null
                },
                Repetitions = new List<int>()
                {
                    10,
                    20
                }
            };

            users.Log(log, "a");

            var result = data.UserExerciseLogs.ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void PrepareForViewShouldReturnCorrectModel()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var users = new UserServices(data);

            var user = new ApplicationUser();
            var workout = WorkoutProvider.TwoWorkouts().First();
            var exercises = new List<Exercise>()
            {
                new Exercise
                {
                    Name = "Squat",
                    Description = "aaaaaaa",
                    EquipmentId = 1
                },
                 new Exercise
                {
                    Name = "Pullup",
                    Description = "aaaaaaa",
                    EquipmentId = 1
                }
            };

            data.Users.Add(user);
            data.Workouts.Add(workout);
            data.Exercises.AddRange(exercises);

            workout.Users.Add(user);

            var workdays = workout.WorkDays.ToList();

            workdays[0].Exercises.Add(exercises[0]);
            workdays[1].Exercises.Add(exercises[1]);
            data.SaveChanges();

            var log = new LogWorkoutForUserServiceModel
            {
                ExerciseIds = new List<string>()
                {
                    workdays[0].Exercises.First().Id,
                    workdays[1].Exercises.First().Id
                },
                Weight = new List<int?>()
                {
                    100,
                    null
                },
                Repetitions = new List<int>()
                {
                    10,
                    20
                }
            };

            users.Log(log, user.Id);

            var result = users.PrepareForView(user.Id);

            Assert.Equal(workout.Name, result.Name);
            Assert.True(result.HasWorkout);
            Assert.Single(result.DisplayExercises);
        }

        [Fact]
        public void CheckIfLoggingUpdatesWorkdays()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var users = new UserServices(data);

            var user = new ApplicationUser();
            var workout = WorkoutProvider.TwoWorkouts().First();
            var exercises = new List<Exercise>()
            {
                new Exercise
                {
                    Name = "Squat",
                    Description = "aaaaaaa",
                    EquipmentId = 1
                },
                 new Exercise
                {
                    Name = "Pullup",
                    Description = "aaaaaaa",
                    EquipmentId = 1
                }
            };

            data.Users.Add(user);
            data.Workouts.Add(workout);
            data.Exercises.AddRange(exercises);

            workout.Users.Add(user);

            var workdays = workout.WorkDays.ToList();

            workdays[0].Exercises.Add(exercises[0]);            

            workdays[1].Exercises.Add(exercises[1]);            

            data.SaveChanges();

            var log = new LogWorkoutForUserServiceModel
            {
                ExerciseIds = new List<string>()
                {
                    workdays[0].Exercises.First().Id,
                    workdays[1].Exercises.First().Id
                },
                Weight = new List<int?>()
                {
                    100,
                    null
                },
                Repetitions = new List<int>()
                {
                    10,
                    20
                }
            };

            users.Log(log, user.Id);

            workdays[0].NextWorkout = workdays[0].NextWorkout.AddDays(-5);
            workdays[1].NextWorkout = workdays[1].NextWorkout.AddDays(-5);
            data.SaveChanges();

            users.PrepareForView(user.Id);

            var result = data.WorkDays.Select(w => w.NextWorkout).ToList();
            
            Assert.True(result[0] == DateTime.Today.AddDays(2) && result[1] == DateTime.Today.AddDays(3));
        }
    }
}
