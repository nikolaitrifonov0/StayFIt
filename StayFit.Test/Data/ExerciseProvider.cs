using StayFit.Data.Models;
using System.Collections.Generic;

namespace StayFit.Test.Data
{
    public static class ExerciseProvider
    {
        public static IEnumerable<Exercise> FourExercises()
        {
            var exercises = new List<Exercise>()
        {
            new Exercise
            {
                Name = "press",
                Description = "asdasd",
                EquipmentId = 1,
                ImageUrl = "aaaa",
                VideoUrl = "https://www.youtube.com/watch?v=qEwKCR5JCog",
                IsPublic = true,
                Equipment = new Equipment { Name = "barbel" },
                BodyParts = new List<BodyPart>()
                {
                    new BodyPart { Name = "shoulder" }
                }
            },
            new Exercise
            {
                Name = "benchpress",
                Description = "asdasd",
                EquipmentId = 1,
                ImageUrl = "aaaa",
                VideoUrl = "aaaa",
                IsPublic = true
            },
            new Exercise
            {
                Name = "deadlift",
                Description = "asdasd",
                EquipmentId = 1,
                ImageUrl = "aaaa",
                VideoUrl = "aaaa",
                IsPublic = false
            },
            new Exercise
            {
                Name = "deadlift",
                Description = "asdasd",
                EquipmentId = 1,
                ImageUrl = "aaaa",
                VideoUrl = "aaaa",
                IsPublic = false
            }
        };

            return exercises;
        }
    }
}
