namespace StayFit.Services.Users
{
    public interface IUserServices
    {
        void Log(LogWorkoutForUserServiceModel workout, string userId);
        LogWorkoutForUserServiceModel PrepareForView(string userId);
    }
}
