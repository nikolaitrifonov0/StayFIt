namespace StayFit.Services.Users
{
    public interface IUserServices
    {
        void Log(LogWorkoutUserServiceModel workout, string userId);
        LogWorkoutUserServiceModel PrepareForView(string userId);
    }
}
