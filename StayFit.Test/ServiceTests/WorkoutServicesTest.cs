using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StayFit.Data;
using StayFit.Services.Workouts;
using StayFit.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using static StayFit.Test.Data.WorkoutProvider;
using static StayFit.Test.Data.ExerciseProvider;
using Microsoft.AspNetCore.Identity;

namespace StayFit.Test.ServiceTests
{
    public class WorkoutServicesTest
    {
        private IMapper mapper;

        public WorkoutServicesTest()
        {
            if (this.mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                this.mapper = mapper;
            }
        }

        [Theory]
        [InlineData("PPL",
            "Tough workout",
            null,
            0,
            "user")]
        public void AddShouldAddWorkoutToDatabaseWithWeeklyType(string name, string description, int? cycleDays,
            int workoutCycleType, string creatorId)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var workouts = new WorkoutServices(data, mapper);

            var exercisesToAdd = FourExercises().ToList();
            data.Exercises.AddRange(exercisesToAdd);
            data.SaveChanges();

            var exercisesToDays = new Dictionary<string, List<string>>();
            exercisesToDays["Monday"] = new List<string>()
            {
                exercisesToAdd[0].Id, exercisesToAdd[1].Id
            };
            exercisesToDays["Tuesday"] = new List<string>()
            {
                exercisesToAdd[2].Id, exercisesToAdd[3].Id
            };

            workouts.Add(name, description, cycleDays, workoutCycleType, creatorId, exercisesToDays);

            Assert.Single(data.Workouts.ToList());
            Assert.Equal(name ,data.Workouts.First().Name);
            Assert.Equal(description, data.Workouts.First().Description);
            Assert.Equal(creatorId, data.Workouts.First().CreatorId);
            Assert.Equal(workoutCycleType, (int)data.Workouts.First().WorkoutCycleType);
            Assert.Equal(2, data.Workouts.First().WorkDays.Count);
            Assert.Equal(2, data.Workouts.Select(w => w.WorkDays.Select(wd => wd.Exercises.ToList().Count).ToList()[0]).First());
            Assert.Equal(2, data.Workouts.Select(w => w.WorkDays.Select(wd => wd.Exercises.ToList().Count).ToList()[1]).First());
        }

        [Theory]
        [InlineData("PPL",
            "Tough workout",
            2,
            1,
            "user")]
        public void AddShouldAddWorkoutToDatabaseWithNthDayType(string name, string description, int? cycleDays,
            int workoutCycleType, string creatorId)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var workouts = new WorkoutServices(data, mapper);

            var exercisesToAdd = FourExercises().ToList();
            data.Exercises.AddRange(exercisesToAdd);
            data.SaveChanges();

            var exercisesToDays = new Dictionary<string, List<string>>();
            exercisesToDays["Day"] = new List<string>()
            {
                exercisesToAdd[0].Id, exercisesToAdd[1].Id
            };
            exercisesToDays["Day2"] = new List<string>()
            {
                exercisesToAdd[2].Id, exercisesToAdd[3].Id
            };

            workouts.Add(name, description, cycleDays, workoutCycleType, creatorId, exercisesToDays);

            Assert.Single(data.Workouts.ToList());
            Assert.Equal(name, data.Workouts.First().Name);
            Assert.Equal(description, data.Workouts.First().Description);
            Assert.Equal(creatorId, data.Workouts.First().CreatorId);
            Assert.Equal(workoutCycleType, (int)data.Workouts.First().WorkoutCycleType);
            Assert.Equal(2, data.Workouts.First().WorkDays.Count);
            Assert.Equal(DateTime.Today.DayOfYear + 1, data.Workouts.First().WorkDays.OrderBy(d => d.NextWorkout).ToList()[0].NextWorkout.DayOfYear);
            Assert.Equal(DateTime.Today.DayOfYear + 4, data.Workouts.First().WorkDays.OrderBy(d => d.NextWorkout).ToList()[1].NextWorkout.DayOfYear);
            Assert.Equal(2, data.Workouts.Select(w => w.WorkDays.Select(wd => wd.Exercises.ToList().Count).ToList()[0]).First());
            Assert.Equal(2, data.Workouts.Select(w => w.WorkDays.Select(wd => wd.Exercises.ToList().Count).ToList()[1]).First());
        }

        [Fact]
        public void AllShouldReturnAllTheWorkouts()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var workouts = new WorkoutServices(data, mapper);

            data.Workouts.AddRange(TwoWorkouts());
            data.SaveChanges();

            Assert.Equal(2, workouts.All().Count());
        }

        [Fact]
        public void AllWithEmptyDatabaseShouldReturnEmptyCollection()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var workouts = new WorkoutServices(data, mapper);

            var all = workouts.All();
            Assert.NotNull(all);
            Assert.Empty(all);
        }

        [Fact]
        public void AssignShouldAddUserToWorkout()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var workouts = new WorkoutServices(data, mapper);

            var workout = TwoWorkouts().First();
            data.Workouts.AddRange(workout);
            
            var user = new IdentityUser
            {
                Id = "user",
            };
            data.Users.Add(user);
            data.SaveChanges();            

            workouts.Assign(user.Id, workout.Id);

            Assert.Single(workout.Users);
            Assert.Equal(user, workout.Users.First());
        }

        [Fact]
        public void WorkoutShouldNotHaveSameUser()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var workouts = new WorkoutServices(data, mapper);

            var workout = TwoWorkouts().First();
            data.Workouts.AddRange(workout);

            var user = new IdentityUser
            {
                Id = "user",
            };
            data.Users.Add(user);
            data.SaveChanges();

            workouts.Assign(user.Id, workout.Id);
            workouts.Assign(user.Id, workout.Id);

            Assert.Single(workout.Users);
            Assert.Equal(user, workout.Users.First());
        }

        [Fact]
        public void WorkoutShouldNotHaveNonExsistentUser()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var workouts = new WorkoutServices(data, mapper);

            var workout = TwoWorkouts().First();
            data.Workouts.AddRange(workout);

            data.SaveChanges();

            workouts.Assign("a", workout.Id);

            Assert.Empty(workout.Users);
        }

        [Fact]
        public void DetailsShouldReturnValidWorkout()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var workouts = new WorkoutServices(data, mapper);

            data.Workouts.AddRange(TwoWorkouts());
            data.SaveChanges();

            var workout = data.Workouts.First();

            var details = workouts.Details(workout.Id);

            Assert.Equal(workout.Id, details.Id);
            Assert.Equal(workout.Name, details.Name);
            Assert.Equal(workout.Description, details.Description);
            Assert.Equal(workout.CreatorId, details.CreatorId);
            Assert.Equal(workout.CycleDays, details.CycleDays);
            Assert.Equal(workout.WorkoutCycleType, details.CycleType);
            Assert.Equal(workout.WorkDays.Count, details.WorkDays.Count);
        }

        [Fact]
        public void DetailsWithInvalidIDShouldReturnNull()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var workouts = new WorkoutServices(data, mapper);           

            var details = workouts.Details("a");

            Assert.Null(details);
        }

        [Theory]
        [InlineData("PPL",
           "Tough workout",
           null,
           0)]
        public void EditShouldChangeWorkout(string name, string description, int? cycleDays,
            int workoutCycleType)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var workouts = new WorkoutServices(data, mapper);

            var exercisesToAdd = FourExercises().ToList();
            data.Exercises.AddRange(exercisesToAdd);

            data.Workouts.AddRange(TwoWorkouts());

            data.SaveChanges();

            var exercisesToDays = new Dictionary<string, List<string>>();
            exercisesToDays["Monday"] = new List<string>()
            {
                exercisesToAdd[0].Id, exercisesToAdd[1].Id
            };
            exercisesToDays["Tuesday"] = new List<string>()
            {
                exercisesToAdd[2].Id, exercisesToAdd[3].Id
            };

            var workout = data.Workouts.Where(w => w.Name == "bbb").First();

            workouts.Edit(workout.Id, name, description, cycleDays, workoutCycleType, exercisesToDays);

            Assert.Equal(2, data.Workouts.ToList().Count);
            Assert.Equal(name, workout.Name);
            Assert.Equal(description, workout.Description);
            Assert.Equal(workoutCycleType, (int)workout.WorkoutCycleType);
            Assert.Equal(2, workout.WorkDays.Count);
            Assert.Equal(2, data.Workouts.Where(w => w.Id == workout.Id).Select(w => w.WorkDays.Select(wd => wd.Exercises.ToList().Count).ToList()[0]).First());
            Assert.Equal(2, data.Workouts.Where(w => w.Id == workout.Id).Select(w => w.WorkDays.Select(wd => wd.Exercises.ToList().Count).ToList()[1]).First());
        }

        [Fact]
        public void EditShouldDoNothingIfWorkoutDoesntExist()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var workouts = new WorkoutServices(data, mapper);

            workouts.Edit("a", "a", "a", 1, 1, null);

            Assert.Empty(data.Workouts.ToList());
        }

        [Fact]
        public void EditShouldDoNothingIfWorkoutTypeDoesntExist()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var workouts = new WorkoutServices(data, mapper);

            var exercisesToAdd = FourExercises().ToList();
            data.Exercises.AddRange(exercisesToAdd);

            data.Workouts.AddRange(TwoWorkouts().First());

            data.SaveChanges();

            var workout = data.Workouts.First();

            workouts.Edit(workout.Id, "aa", "aa", 1, 3, new Dictionary<string, List<string>>());

            Assert.Single(data.Workouts.ToList());
            Assert.Equal(workout.Name, data.Workouts.First().Name);
            Assert.Equal(workout.Description, data.Workouts.First().Description);
            Assert.Equal(workout.CreatorId, data.Workouts.First().CreatorId);
            Assert.Equal(workout.WorkoutCycleType, data.Workouts.First().WorkoutCycleType);
            Assert.Equal(2, data.Workouts.First().WorkDays.Count);
        }

        [Fact]
        public void EditDetailsShouldReturnCorrectWorkout()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var workouts = new WorkoutServices(data, mapper);

            data.Workouts.AddRange(TwoWorkouts());
            data.SaveChanges();

            var workout = data.Workouts.First();

            var details = workouts.EditDetails(workout.Id);

            Assert.Equal(workout.Name, details.Name);
            Assert.Equal(workout.Description, details.Description);
            Assert.Equal(workout.CycleDays, details.CycleDays);
            Assert.Equal((int)workout.WorkoutCycleType, details.WorkoutCycleType);
        }

        [Fact]
        public void EditDetailsWithInvalidIdShouldReturnNull()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var workouts = new WorkoutServices(data, mapper);

            var details = workouts.EditDetails("a");

            Assert.Null(details);
        }

        [Fact]
        public void IsCreatorShouldReturnTrue()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var workouts = new WorkoutServices(data, mapper);

            data.Workouts.AddRange(TwoWorkouts());
            data.SaveChanges();

            var user = data.Users.Where(u => u.UserName == "user").First();
            var workout = data.Workouts.Where(w => w.Name == "aaa").First();


            Assert.True(workouts.IsCreator(workout.Id, user.Id));
        }

        [Fact]
        public void IsCreatorShouldReturnFalseIfNotCreator()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var workouts = new WorkoutServices(data, mapper);

            data.Workouts.AddRange(TwoWorkouts());
            data.SaveChanges();

            var user = data.Users.Where(u => u.UserName == "user1").First();
            var workout = data.Workouts.Where(w => w.Name == "aaa").First();


            Assert.False(workouts.IsCreator(workout.Id, user.Id));
        }

        [Fact]
        public void IsCreatorShouldReturnFalseIfUserDoesntExist()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var workouts = new WorkoutServices(data, mapper);

            data.Workouts.AddRange(TwoWorkouts());
            data.SaveChanges();

            var workout = data.Workouts.Where(w => w.Name == "aaa").First();


            Assert.False(workouts.IsCreator(workout.Id, "a"));
        }

        [Fact]
        public void IsCreatorShouldReturnFalseIfWorkoutDoesntExist()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var workouts = new WorkoutServices(data, mapper);

            data.Workouts.AddRange(TwoWorkouts());
            data.SaveChanges();

            var user = data.Users.Where(u => u.UserName == "user1").First();


            Assert.False(workouts.IsCreator("a", user.Id));
        }
    }
}
