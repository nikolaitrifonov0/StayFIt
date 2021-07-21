using System.Collections.Generic;

namespace StayFit.Services.Exercises
{
    public interface IExerciseService
    {
        IEnumerable<ExerciseSearchServiceModel> Find(string keyword);
    }
}
