namespace StayFit.Services.Workouts
{
    public class AllWorkoutsServiceModel
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string Creator { get; init; }
        public int TotalWorkDays { get; init; }
        public string ImageUrl { get; set; }
    }
}
