using StayFit.Data;
using System.Collections.Generic;
using System.Linq;

namespace StayFit.Services.Exercises
{
    public class ExerciseService : IExerciseService
    {
        private readonly StayFitContext data;

        public ExerciseService(StayFitContext data) 
            => this.data = data;

        public IEnumerable<ExerciseSearchServiceModel> Find(string keyword)
            => data.Exercises
            .Select(e => new ExerciseSearchServiceModel { Id = e.Id, Name = e.Name })
            .Where(e => e.Name.Contains(keyword))
            .ToList();            
        
    }
}
