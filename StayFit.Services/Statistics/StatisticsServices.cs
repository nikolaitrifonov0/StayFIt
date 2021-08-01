using StayFit.Data;
using System.Collections.Generic;
using System.Linq;

namespace StayFit.Services.Statistics
{
    public class StatisticsServices : IStatisticsServices
    {
        private readonly StayFitContext data;

        public StatisticsServices(StayFitContext data)
        {
            this.data = data;
        }

        public StatisticsServiceModel GetAll(string userId)
            => new StatisticsServiceModel
            {
                UserMaxScores = this.GetUserMaxScores(userId),
                UserMaxWeights = this.GetUserMaxWeights(userId),
                UserWorkoutProgress = this.GetUserWorkoutProgress(userId)
            };

        public Dictionary<string, List<ScoreProgressServiceModel>> GetUserMaxScores(string userId)
        {
            var logs = this.data.UserExerciseLogs
                .Where(uel => uel.UserId == userId)
                .GroupBy(uel => new { uel.Date, uel.Exercise.Name })
                .Select(uel => new
                {
                    Exercise = uel.Key.Name,
                    Date = uel.Key.Date,
                    Score = uel.Max(uel => uel.Weight != null ? uel.Weight * uel.Repetitions : uel.Repetitions)
                }).OrderByDescending(uel => uel.Score).ToList();


            var result = new Dictionary<string, List<ScoreProgressServiceModel>>();

            foreach (var log in logs)
            {
                if (!result.ContainsKey(log.Exercise))
                {
                    result[log.Exercise] = new List<ScoreProgressServiceModel>();
                }

                result[log.Exercise].Add(new ScoreProgressServiceModel
                {
                    Date = log.Date,
                    Score = log.Score.Value
                });
            }

            return result;
        }

        public Dictionary<string, List<WeightProgressServiceModel>> GetUserMaxWeights(string userId)
        {
            var logs = this.data.UserExerciseLogs
                .Where(uel => uel.UserId == userId)
                .GroupBy(uel => new { uel.Date, uel.Exercise.Name })
                .Select(uel => new 
                {
                    Exercise = uel.Key.Name,
                    Date = uel.Key.Date,
                    Weight = uel.Max(uel => uel.Weight)
                }).OrderByDescending(uel => uel.Weight).ToList();


            var result = new Dictionary<string, List<WeightProgressServiceModel>>();
            
            foreach (var log in logs)
            {
                if (!result.ContainsKey(log.Exercise))
                {
                    result[log.Exercise] = new List<WeightProgressServiceModel>();
                }

                result[log.Exercise].Add(new WeightProgressServiceModel
                {
                    Date = log.Date,
                    Weight = log.Weight
                });
            }

            return result;
        }

        public IEnumerable<WorkoutProgressScore> GetUserWorkoutProgress(string userId)
        {
            var logs = this.data.UserExerciseLogs
                .Select(uel => new 
                { 
                    uel.WorkDay.Workout.Users,
                    uel.Date,
                    uel.Weight,
                    uel.Repetitions
                })
                .Where(uel => uel.Users.Any(u => u.Id == userId))
                .ToList()
                .GroupBy(uel => uel.Date)
                .Select(uel => new WorkoutProgressScore
                {
                    Date = uel.Key,
                    Score = uel.Sum(uel => uel.Weight != null ? uel.Weight.Value * uel.Repetitions : uel.Repetitions)
                }).ToList();

            return logs;
        }
    }
}
