using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StayFit.Data;
using StayFit.Data.Models;
using StayFit.Data.Models.Enums.Workout;
using StayFit.Services.Exercises;
using StayFit.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using static StayFit.Test.Data.ExerciseProvider;

namespace StayFit.Test.ServiceTests
{
    public class ExerciseServicesTest
    {
        private IMapper mapper;

        public ExerciseServicesTest()
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
        [InlineData("My Exercise",
            "The best exercise for the back",            
            "https://image.shutterstock.com/image-vector/sequence-weightlifter-doing-deadlift-exercise-260nw-1482332555.jpg",
            "https://www.youtube.com/watch?v=r4MzxtBKyNE",
            4,
            new int[0])]
        public void AddShouldAddValidExercise(string name, string description, string imageUrl,
            string videoUrl, int equipment, IEnumerable<int> bodyParts)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var exercises = new ExerciseServices(data, mapper);

            exercises.Add(name, description, imageUrl,
            videoUrl, equipment, bodyParts);

            var exercise = data.Exercises.Where(e => e.Name == name).FirstOrDefault();

            Assert.Single(data.Exercises.ToList());
            Assert.NotNull(exercise);
            Assert.Equal(exercise.Name, name);
            Assert.Equal(exercise.Description, description);
            Assert.Equal(exercise.ImageUrl, imageUrl);
            Assert.Equal(exercise.VideoUrl, videoUrl);
        }

        [Fact]
        public void AllWithPublicOnlyShouldReturnAllPublicExercises()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var exercises = new ExerciseServices(data, mapper);

            data.Exercises.AddRange(FourExercises());
            data.SaveChanges();

            var result = exercises.All();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void AllWithoutPublicOnlyShouldReturnAllExercises()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var exercises = new ExerciseServices(data, mapper);

            data.Exercises.AddRange(FourExercises());
            data.SaveChanges();

            var result = exercises.All(false);

            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void AllWithEmptyDatabaseShouldReturnEmptyCollection()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var exercises = new ExerciseServices(data, mapper);           

            var result = exercises.All();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void DetailsShouldReturnCorrectExercise()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var exercises = new ExerciseServices(data, mapper);

            data.Exercises.AddRange(FourExercises());
            data.SaveChanges();

            var id = data.Exercises.Where(e => e.Name == "press").First().Id;

            var exercise = exercises.Details(id);

            Assert.Equal("press", exercise.Name);
            Assert.Equal(id, exercise.Id);
        }

        [Fact]
        public void DetailsWithInvalidIdShouldReturnNull()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var exercises = new ExerciseServices(data, mapper);

            data.Exercises.AddRange(FourExercises());
            data.SaveChanges();            

            var exercise = exercises.Details("0");

            Assert.Null(exercise);
        }

        [Theory]
        [InlineData("My Exercise",
            "The best exercise for the back",
            "https://image.shutterstock.com/image-vector/sequence-weightlifter-doing-deadlift-exercise-260nw-1482332555.jpg",
            "https://www.youtube.com/watch?v=r4MzxtBKyNE",
            4,
            new int[0])]
        public void EditShouldChangeProperties(string name, string description,
            string imageUrl, string videoUrl, int equipment, IEnumerable<int> bodyParts)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var exercises = new ExerciseServices(data, mapper);

            data.Exercises.AddRange(FourExercises());
            data.SaveChanges();

            var exerciseToEdit = data.Exercises.First();

            exercises.Edit(exerciseToEdit.Id, name, description,
                imageUrl, videoUrl, equipment, bodyParts);

            exerciseToEdit = data.Exercises.Find(exerciseToEdit.Id);

            Assert.Equal(name, exerciseToEdit.Name);
            Assert.Equal(description, exerciseToEdit.Description);
            Assert.Equal(imageUrl, exerciseToEdit.ImageUrl);
            Assert.Equal(videoUrl, exerciseToEdit.VideoUrl);
            Assert.Equal(equipment, exerciseToEdit.EquipmentId);
            Assert.Equal(0, exerciseToEdit.BodyParts.Count);
        }

        [Theory]
        [InlineData("My Exercise",
            "The best exercise for the back",
            "https://image.shutterstock.com/image-vector/sequence-weightlifter-doing-deadlift-exercise-260nw-1482332555.jpg",
            "https://www.youtube.com/watch?v=r4MzxtBKyNE",
            4,
            new int[0])]
        public void EditShouldDoNothingIfExerciseDoesntExist(string name, string description,
            string imageUrl, string videoUrl, int equipment, IEnumerable<int> bodyParts)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var exercises = new ExerciseServices(data, mapper);

            data.Exercises.AddRange(FourExercises());
            data.SaveChanges();

            exercises.Edit("1", name, description,
                imageUrl, videoUrl, equipment, bodyParts);

            Assert.Null(data.Exercises.Where(e => e.Name == name).FirstOrDefault());
        }

        [Fact]
        public void EditDetailsShouldReturnCorrectExercise()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var exercises = new ExerciseServices(data, mapper);

            data.Exercises.AddRange(FourExercises());
            data.SaveChanges();

            var id = data.Exercises.Where(e => e.Name == "press").First().Id;

            var exercise = exercises.EditDetails(id);

            Assert.Equal("press", exercise.Name);
            Assert.Equal("asdasd", exercise.Description);
        }

        [Fact]
        public void EditDetailsWithInvalidIdShouldReturnNull()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var exercises = new ExerciseServices(data, mapper);

            data.Exercises.AddRange(FourExercises());
            data.SaveChanges();

            var exercise = exercises.EditDetails("0");

            Assert.Null(exercise);
        }

        [Fact]
        public void FindShouldReturnAllMatchingExercises()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var exercises = new ExerciseServices(data, mapper);

            data.Exercises.AddRange(FourExercises());
            data.SaveChanges();

            var result = exercises.Find("pre");

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void FindShouldReturnEmptyCollectionIfNoMatches()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var exercises = new ExerciseServices(data, mapper);

            data.Exercises.AddRange(FourExercises());
            data.SaveChanges();

            var result = exercises.Find("prgf");

            Assert.Empty(result);
        }

        [Fact]
        public void FindShouldReturnEmptyCollectionIfLessThan3Chars()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var exercises = new ExerciseServices(data, mapper);

            data.Exercises.AddRange(FourExercises());
            data.SaveChanges();

            var result = exercises.Find("pr");

            Assert.Empty(result);
        }

        [Fact]
        public void IsInWorkoutShouldReturnTrue()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var exercises = new ExerciseServices(data, mapper);
            var user = new IdentityUser();

            data.Exercises.AddRange(FourExercises());

            data.SaveChanges();

            var exerciseToFind = data.Exercises.Where(e => e.Name == "benchpress").First();

            var workout = new Workout
            {
                Name = "Fit",
                Description = "aaaaaaaaaaa",
                WorkoutCycleType = WorkoutCycleType.Weekly,
                
                WorkDays = new List<WorkDay>()
                {
                    new WorkDay
                    {
                        NextWorkout = DateTime.Today,
                    }
                }
            };

            workout.WorkDays.First().Exercises.Add(exerciseToFind);
            workout.Users.Add(user);

            data.Workouts.Add(workout);

            data.SaveChanges();

            Assert.True(exercises.IsInWorkout(exerciseToFind.Id, workout.Users.First().Id));
        }

        [Fact]
        public void IsInWorkoutShouldReturnFalseIfUserDoesntExist()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var exercises = new ExerciseServices(data, mapper);

            data.Exercises.AddRange(FourExercises());

            data.SaveChanges();

            var exerciseToFind = data.Exercises.Where(e => e.Name == "benchpress").First();

            var workout = new Workout
            {
                Name = "Fit",
                Description = "aaaaaaaaaaa",
                WorkoutCycleType = WorkoutCycleType.Weekly,

                WorkDays = new List<WorkDay>()
                {
                    new WorkDay
                    {
                        NextWorkout = DateTime.Today,
                    }
                }
            };

            workout.WorkDays.First().Exercises.Add(exerciseToFind);

            data.Workouts.Add(workout);

            data.SaveChanges();

            Assert.False(exercises.IsInWorkout(exerciseToFind.Id, "0"));
        }

        [Fact]
        public void IsInWorkoutShouldReturnFalseIfExerciseDoesntExist()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var exercises = new ExerciseServices(data, mapper);
            var user = new IdentityUser();

            var workout = new Workout
            {
                Name = "Fit",
                Description = "aaaaaaaaaaa",
                WorkoutCycleType = WorkoutCycleType.Weekly,

                WorkDays = new List<WorkDay>()
                {
                    new WorkDay
                    {
                        NextWorkout = DateTime.Today,
                    }
                }
            };            

            data.Workouts.Add(workout);

            workout.Users.Add(user);

            data.SaveChanges();

            Assert.False(exercises.IsInWorkout("0", user.Id));
        }

        [Fact]
        public void ShowShouldMakeExercisePublic()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var exercises = new ExerciseServices(data, mapper);

            data.Exercises.AddRange(FourExercises());

            data.SaveChanges();

            var exercise = data.Exercises.Where(e => e.Name == "deadlift").First();
            exercises.Show(exercise.Id);

            Assert.True(exercise.IsPublic);
        }

        [Fact]
        public void HideShouldMakeExerciseNotPublic()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var exercises = new ExerciseServices(data, mapper);

            data.Exercises.AddRange(FourExercises());

            data.SaveChanges();

            var exercise = data.Exercises.Where(e => e.Name == "benchpress").First();
            exercises.Hide(exercise.Id);

            Assert.False(exercise.IsPublic);
        }        
    }    
}
