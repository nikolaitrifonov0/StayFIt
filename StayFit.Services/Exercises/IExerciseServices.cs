using System.Collections.Generic;

namespace StayFit.Services.Exercises
{
    public interface IExerciseServices
    {
        IEnumerable<ExerciseSearchServiceModel> Find(string keyword);
        ExerciseDetailsServiceModel Details(string id);
        void Add(string name,
        string description,
        string imageUrl,
        string videoUrl,
        int equipment,
        IEnumerable<int> bodyParts);
    }
}
