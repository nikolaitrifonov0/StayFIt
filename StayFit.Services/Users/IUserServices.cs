namespace StayFit.Services.Users
{
    public interface IUserServices
    {
        void Add(LogWorkoutUserServiceModel workout, string userId);
        LogWorkoutUserServiceModel PrepareForView(string userId);
    }
}
