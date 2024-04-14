using System.Collections.Generic;

namespace StayFit.Services.Statistics
{
    public class StatisticsServiceModel
    {
        public Dictionary<string, List<WeightProgressServiceModel>> UserMaxWeights { get; init; }
        public Dictionary<string, List<ScoreProgressServiceModel>> UserMaxScores { get; init; }
        public Dictionary<string, List<UserStatisticsHighScoreModel>> UsersHighScores { get; init; }
        public IEnumerable<WorkoutProgressScore> UserWorkoutProgress { get; init; }
    }
}