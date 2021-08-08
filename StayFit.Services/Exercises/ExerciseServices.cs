using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StayFit.Data;
using StayFit.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StayFit.Services.Exercises
{
    public class ExerciseServices : IExerciseServices
    {
        private readonly StayFitContext data;
        private readonly IConfigurationProvider mapper;

        public ExerciseServices(StayFitContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public void Add(string name, string description, string imageUrl, 
            string videoUrl, int equipment, IEnumerable<int> bodyParts)
        {
            var exercise = new Exercise
            {
                Name = name,
                Description = description,
                ImageUrl = imageUrl,
                VideoUrl = videoUrl,
                EquipmentId = equipment,
                IsPublic = false
            };

            foreach (var bodyPart in bodyParts)
            {
                exercise.BodyParts.Add(data.BodyParts.Find(bodyPart));
            }

            this.data.Exercises.Add(exercise);
            this.data.SaveChanges();
        }

        public IEnumerable<ExerciseSearchServiceModel> All(bool publicOnly = true) 
            => this.data
            .Exercises
            .Where(e => !publicOnly || e.IsPublic)
            .ProjectTo<ExerciseSearchServiceModel>(this.mapper)
            .ToList();

        public ExerciseDetailsServiceModel Details(string id) 
            => this.data
                .Exercises
                .Where(e => e.Id == id)
                .ProjectTo<ExerciseDetailsServiceModel>(this.mapper)
                .FirstOrDefault();

        public void Edit(string id,string name, string description, 
            string imageUrl, string videoUrl, int equipment, IEnumerable<int> bodyParts)
        {
            var exercise = this.data.Exercises.Find(id);
            var exerciseBodyParts = this.data.Exercises
                .Where(e => e.Id == id)
                .Select(e => e.BodyParts.Select(b => b.Id).ToList())
                .FirstOrDefault();

            exercise.Name = name;
            exercise.Description = description;
            exercise.ImageUrl = imageUrl;
            exercise.VideoUrl = videoUrl;
            exercise.EquipmentId = equipment;
            exercise.IsPublic = false;

            foreach (var bodyPart in bodyParts)
            {
                if (!exerciseBodyParts.Any(x => x == bodyPart))
                {
                    exercise.BodyParts.Add(this.data.BodyParts.Find(bodyPart));
                }
            }            
            
            foreach (var bodyPart in exerciseBodyParts)
            {
                if (!bodyParts.Any(b => b == bodyPart))
                {
                    var toDelete = this.data.BodyParts.Find(bodyPart);
                    this.data.Exercises.Include(e => e.BodyParts).FirstOrDefault(e => e.Id == id).BodyParts.Remove(toDelete);      
                }
            }

            this.data.SaveChanges();            
        }

        public ExerciseEditServiceModel EditDetails(string exerciseId) 
            => this.data.Exercises
                .Where(e => e.Id == exerciseId)
                .ProjectTo<ExerciseEditServiceModel>(this.mapper)
                .FirstOrDefault();

        public IEnumerable<ExerciseSearchServiceModel> Find(string keyword)
            => data.Exercises
            .ProjectTo<ExerciseSearchServiceModel>(this.mapper)
            .Where(e => e.Name.Contains(keyword) && e.IsPublic)
            .ToList();

        public bool IsInWorkout(string exerciseId, string userId) =>
           this.data.WorkDays
           .Where(wd => wd.Workout.Users.Any(u => u.Id == userId)
               && wd.NextWorkout.DayOfYear == DateTime.Today.DayOfYear)
           .Select(wd => new { wd.Exercises })
           .First()
           .Exercises.Any(e => e.Id == exerciseId);

        public void Hide(string Id)
        {
            this.data.Exercises.Find(Id).IsPublic = false;
            this.data.SaveChanges();
        }       

        public void Show(string Id)
        {
            this.data.Exercises.Find(Id).IsPublic = true;
            this.data.SaveChanges();
        }
    }
}
