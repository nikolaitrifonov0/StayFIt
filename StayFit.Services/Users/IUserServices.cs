using StayFit.Data.Models;

namespace StayFit.Services.Users
{
    public interface IUserServices
    {
        void Log(LogWorkoutForUserServiceModel workout, string userId);
        LogWorkoutForUserServiceModel PrepareForView(string userId);
        void AssignNextWorkDay(ApplicationUser user, Workout workout, bool isNewWorkout = false);
        void MoveWorkoutToToday(string userId);
    }
}
