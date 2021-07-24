using System.Collections.Generic;

namespace StayFit.Services.Workouts
{
    public interface IWorkoutServices
    {
        AllWorkoutsServiceModel All();
        void Add(string name,
                string description,
                int? cycleDays,
                int workoutCycleType,
                string creatorId,
                Dictionary<string, List<string>> exercisesToDays);
        WorkoutDetailsServiceModel Details(string id);
        void Assign(string userId, string workoutId);
    }
}
