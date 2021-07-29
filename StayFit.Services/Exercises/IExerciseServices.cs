using System.Collections.Generic;

namespace StayFit.Services.Exercises
{
    public interface IExerciseServices
    {
        public IEnumerable<ExerciseSearchServiceModel> Find(string keyword);
        public ExerciseDetailsServiceModel Details(string id);
        void Add(string name,
        string description,
        string imageUrl,
        string videoUrl,
        int equipment,
        IEnumerable<int> bodyParts);
        public bool IsInWorkout(string exerciseId, string userId);
        public IEnumerable<ExerciseSearchServiceModel> All();

    }
}
