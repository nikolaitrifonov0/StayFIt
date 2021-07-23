using StayFit.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StayFit.Services.Exercises
{
    public class ExerciseService : IExerciseService
    {
        private readonly StayFitContext data;

        public ExerciseService(StayFitContext data) 
            => this.data = data;

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
