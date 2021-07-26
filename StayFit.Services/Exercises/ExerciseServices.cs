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

        public ExerciseServices(StayFitContext data) => this.data = data;

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

        public ExerciseDetailsServiceModel Details(string id)
        {
            var youtubeEmbedLink = "https://www.youtube.com/embed/";
            var youtubeLinkSeparator = "v=";

            return this.data
                    .Exercises
                    .Where(e => e.Id == id)
                    .Select(e => new ExerciseDetailsServiceModel
                    {
                        Name = e.Name,
                        Description = e.Description,
                        ImageUrl = e.ImageUrl,
                        VideoUrl = youtubeEmbedLink + e.VideoUrl.Split(youtubeLinkSeparator, StringSplitOptions.RemoveEmptyEntries)[1],
                        Equipment = e.Equipment.Name,
                        BodyParts = e.BodyParts.Select(bp => bp.Name).ToList()
                    }).FirstOrDefault();
        }

        public IEnumerable<ExerciseSearchServiceModel> Find(string keyword)
            => data.Exercises
            .Select(e => new ExerciseSearchServiceModel { Id = e.Id, Name = e.Name })
            .Where(e => e.Name.Contains(keyword))
            .ToList();
    }
}
