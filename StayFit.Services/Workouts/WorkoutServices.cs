using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StayFit.Data;
using StayFit.Data.Models;
using StayFit.Data.Models.Enums.Workout;
using StayFit.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StayFit.Services.Workouts
{
    public class WorkoutServices : IWorkoutServices
    {
        private readonly StayFitContext data;
        private readonly IConfigurationProvider mapper;
        private readonly IUserServices userServices;

        public WorkoutServices(StayFitContext data, IMapper mapper, IUserServices userServices)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
            this.userServices = userServices;
        }

        public void Add(string name, string description, int? cycleDays, 
            int workoutCycleType, string creatorId, Dictionary<string, List<string>> exercisesToDays)
        {
            var workout = new Workout
            {
                Name = name,
                Description = description,
                CycleDays = cycleDays,
                WorkoutCycleType = workoutCycleType == 0 ?
                WorkoutCycleType.Weekly : WorkoutCycleType.EveryNDays,
                CreatorId = creatorId
            };

            foreach (var ed in exercisesToDays)
            {
                var workDay = new WorkDay
                {
                    Workout = workout,
                    Exercises = this.data.Exercises.Where(e => ed.Value.Contains(e.Id)).ToHashSet(),
                    DayOfWeek = workout.WorkoutCycleType == WorkoutCycleType.Weekly ? Enum.Parse<DayOfWeek>(ed.Key) : null
                };

                workout.WorkDays.Add(workDay);
            }

            this.data.Workouts.Add(workout);
            this.data.SaveChanges();
        }

        public IEnumerable<AllWorkoutsServiceModel> All()
            => this.data.Workouts
            .ProjectTo<AllWorkoutsServiceModel>(this.mapper)
            .ToList();
     

        public void Assign(string userId, string workoutId)
        {
            var user = this.data.Users.Where(u => u.Id == userId).FirstOrDefault();
            var workout = this.data.Workouts.Where(w => w.Id == workoutId).FirstOrDefault();

            if (workout == null || user == null || workout.Users.Any(u => u.Id == userId))
            {
                return;
            }

            workout.Users.Add(user);
            data.SaveChanges();

            userServices.AssignNextWorkDay(user, workout, true);
        }

        public WorkoutDetailsServiceModel Details(string id)
        {
            if (!this.data.Workouts.Any(w => w.Id == id))
            {
                return null;
            }

            int dayNumber = 1;
            var workout = this.data.Workouts
                .Include(w => w.WorkDays)
                .ThenInclude(w => w.Exercises)
                .AsEnumerable()
                .Where(w => w.Id == id)
                .Select(w => new WorkoutDetailsServiceModel
                {
                    Id = w.Id,
                    Name = w.Name,
                    Description = w.Description,
                    CycleType = w.WorkoutCycleType,
                    CycleDays = w.CycleDays,
                    CreatorId = w.CreatorId,
                    WorkDays = w.WorkDays.Select(wd => new DetailsWorkDayServiceModel
                    {
                        Exercises = wd.Exercises
                        .Select(e => new { e.Id, e.Name }).ToDictionary(e => e.Id, e => e.Name),
                        Day = w.WorkoutCycleType == WorkoutCycleType.Weekly ? wd.DayOfWeek.ToString() : $"Day {dayNumber++}"
                    }).ToList()
                }).FirstOrDefault();            

            return workout;
        }

        public void Edit(string id, string name, string description, int? cycleDays, 
            int workoutCycleType, string imageUrl, Dictionary<string, List<string>> exercisesToDays)
        {
            var workout = this.data.Workouts.Include(w => w.Users).First(w => w.Id == id);

            if (workout == null)
            {
                return;
            }

            var users = workout.Users;

            foreach (var user in users)
            {
                user.NextWorkDayId = null;
                user.NextWorkout = null;
                workout.Users.Remove(user);
            }

            this.data.SaveChanges();

            workout.Name = name;
            workout.Description = description;
            workout.CycleDays = cycleDays;
            workout.ImageUrl = imageUrl;

            if (workoutCycleType == 0)
            {
                workout.WorkoutCycleType = WorkoutCycleType.Weekly;
            }
            else if (workoutCycleType == 1)
            {
                workout.WorkoutCycleType = WorkoutCycleType.EveryNDays;
            }
            else
            {
                return;
            }

            if (exercisesToDays != null)
            {
                var workDays = this.data.WorkDays
                    .Where(wd => wd.WorkoutId == workout.Id).ToList();                

                this.data.WorkDays.RemoveRange(workDays);               

                foreach (var ed in exercisesToDays)
                {
                    var workDay = new WorkDay
                    {
                        Workout = workout,
                        Exercises = this.data.Exercises.Where(e => ed.Value.Contains(e.Id)).ToHashSet(),
                        DayOfWeek = Enum.Parse<DayOfWeek>(ed.Key)
                    };

                    workout.WorkDays.Add(workDay);
                }                
            }

            this.data.SaveChanges();
        }

        public EditWorkoutsServiceModel EditDetails(string id) 
            => this.data.Workouts.Where(w => w.Id == id)
                .ProjectTo<EditWorkoutsServiceModel>(this.mapper)
                .FirstOrDefault();

        public bool IsCreator(string workoutId, string userId)
            => this.data.Workouts.Find(workoutId)?.CreatorId == userId;
    }
}
