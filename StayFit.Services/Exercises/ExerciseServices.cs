using Microsoft.EntityFrameworkCore;
using StayFit.Data;
using StayFit.Data.Models;
using StayFit.Services.BodyParts;
using StayFit.Services.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StayFit.Services.Exercises
{
    public class ExerciseServices : IExerciseServices
    {
        private readonly StayFitContext data;
        private readonly IEquipmentServices equipments;
        private readonly IBodyPartServices bodyParts;

        public ExerciseServices(StayFitContext data, IEquipmentServices equipments, IBodyPartServices bodyParts)
        {
            this.data = data;
            this.equipments = equipments;
            this.bodyParts = bodyParts;
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
                EquipmentId = equipment
            };

            foreach (var bodyPart in bodyParts)
            {
                exercise.BodyParts.Add(data.BodyParts.Find(bodyPart));
            }

            this.data.Exercises.Add(exercise);
            this.data.SaveChanges();
        }

        public IEnumerable<ExerciseSearchServiceModel> All() 
            => this.data
            .Exercises
            .Select(e => new ExerciseSearchServiceModel { Id = e.Id, Name = e.Name })
            .ToList();

        public ExerciseDetailsServiceModel Details(string id)
        {
            var youtubeEmbedLink = "https://www.youtube.com/embed/";
            var youtubeLinkSeparator = "v=";

            return this.data
                    .Exercises
                    .Where(e => e.Id == id)
                    .Select(e => new ExerciseDetailsServiceModel
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Description = e.Description,
                        ImageUrl = e.ImageUrl,
                        VideoUrl = youtubeEmbedLink + e.VideoUrl.Split(youtubeLinkSeparator, StringSplitOptions.RemoveEmptyEntries)[1],
                        Equipment = e.Equipment.Name,
                        BodyParts = e.BodyParts.Select(bp => bp.Name).ToList()
                    }).FirstOrDefault();
        }

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
        {
           return this.data.Exercises.Where(e => e.Id == exerciseId)
                .Select(e => new ExerciseEditServiceModel
                {
                    Name = e.Name,
                    Description = e.Description,
                    BodyParts = e.BodyParts.Select(b => b.Id).ToList(),
                    Equipment = e.Equipment.Id,
                    ImageUrl = e.ImageUrl,
                    VideoUrl = e.VideoUrl
                })
                .FirstOrDefault();
        }

        public IEnumerable<ExerciseSearchServiceModel> Find(string keyword)
            => data.Exercises
            .Select(e => new ExerciseSearchServiceModel { Id = e.Id, Name = e.Name })
            .Where(e => e.Name.Contains(keyword))
            .ToList();

        public bool IsInWorkout(string exerciseId, string userId) =>
            this.data.WorkDays
            .Where(wd => wd.Workout.Users.Any(u => u.Id == userId)
                && wd.NextWorkout.DayOfYear == DateTime.Today.DayOfYear)
            .Select(wd => new { wd.Exercises })
            .First()
            .Exercises.Any(e => e.Id == exerciseId);
    }
}
