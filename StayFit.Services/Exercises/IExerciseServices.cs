using System.Collections.Generic;

namespace StayFit.Services.Exercises
{
    public interface IExerciseServices
    {
        public IEnumerable<ExerciseSearchServiceModel> Find(string keyword);
        public ExerciseDetailsServiceModel Details(string id);
        public void Add(string name,
        string description,
        string imageUrl,
        string videoUrl,
        int equipment,
        IEnumerable<int> bodyParts);
        public void Edit(string id,
        string name,
        string description,
        string imageUrl,
        string videoUrl,
        int equipment,
        IEnumerable<int> bodyParts);
        public ExerciseEditServiceModel EditDetails(string exerciseId);
        public bool IsInWorkout(string exerciseId, string userId);
        public IEnumerable<ExerciseSearchServiceModel> All(bool publicOnly = true);
        public void Hide(string Id);
        public void Show(string Id);

    }
}
