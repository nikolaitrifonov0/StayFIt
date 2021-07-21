namespace StayFit.Services.Workouts
{
    public interface IWorkoutServices
    {
        AllWorkoutsServiceModel All();
        WorkoutDetailsServiceModel Details(string id);
        void Assign(string userId, string workoutId);
    }
}
