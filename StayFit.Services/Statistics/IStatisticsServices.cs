using System.Collections.Generic;

namespace StayFit.Services.Statistics
{
    public interface IStatisticsServices
    {
        public StatisticsServiceModel GetAll(string userId);
        public Dictionary<string, List<WeightProgressServiceModel>> GetUserMaxWeights(string userId);
        public Dictionary<string, List<ScoreProgressServiceModel>> GetUserMaxScores(string userId);
        public IEnumerable<WorkoutProgressScore> GetUserWorkoutProgress(string userId);
    }
}
