﻿using System.Collections.Generic;

namespace StayFit.Services.Workouts
{
    public interface IWorkoutServices
    {
        public AllWorkoutsServiceModel All();
        public void Add(string name,
                string description,
                int? cycleDays,
                int workoutCycleType,
                string creatorId,
                Dictionary<string, List<string>> exercisesToDays);
        public void Edit(string id,
                string name,
                string description,
                int? cycleDays,
                int workoutCycleType,
                Dictionary<string, List<string>> exercisesToDays);
        public WorkoutEditServiceModel Details(string id);
        public EditWorkoutsServiceModel EditDetails(string id);
        public void Assign(string userId, string workoutId);
        public bool IsCreator(string workoutId, string userId);
    }
}
