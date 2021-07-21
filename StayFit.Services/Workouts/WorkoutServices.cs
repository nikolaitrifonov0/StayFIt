using StayFit.Data;
using System.Linq;

namespace StayFit.Services.Workouts
{
    public class WorkoutServices : IWorkoutServices
    {
        private readonly StayFitContext data;

        public WorkoutServices(StayFitContext data)
        {
            this.data = data;
        }

        public AllWorkoutsServiceModel All() => new AllWorkoutsServiceModel
        {
            Workouts = this.data.Workouts.Select(w => new WorkoutAllServiceModel
            {
                Id = w.Id,
                Name = w.Name,
                Description = w.Description,
                Creator = w.Creator.UserName,
                TotalWorkDays = w.WorkDays.Count
            }).ToList()
        }; 
        

    }
}
