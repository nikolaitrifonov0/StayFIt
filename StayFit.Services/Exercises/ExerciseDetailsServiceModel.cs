using System.Collections.Generic;

namespace StayFit.Services.Exercises
{
    public class ExerciseDetailsServiceModel
    {
        public string Id { get; init; }
        public string Name { get; init; }        
        public string Description { get; init; }
        public string Equipment { get; init; }
        public ICollection<string> BodyParts { get; init; }
        public string ImageUrl { get; init; }
        public string VideoUrl { get; init; }
    }
}
