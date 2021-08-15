using Microsoft.AspNetCore.Identity;
using StayFit.Data.Models;
using StayFit.Data.Models.Enums.Workout;
using System;
using System.Collections.Generic;

namespace StayFit.Test.Data
{
    public class WorkoutProvider
    {
        public static IEnumerable<Workout> TwoWorkouts() => new List<Workout>()
            {
                new Workout
                {
                    Name = "aaa",
                    Description = "aaaaaaaaa",
                    Creator = new IdentityUser
                    {
                        UserName = "user"
                    },
                    WorkoutCycleType = WorkoutCycleType.Weekly,
                    WorkDays = new List<WorkDay>()
                    {
                        new WorkDay()
                        {
                            NextWorkout = DateTime.Today
                        },
                        new WorkDay()
                        {
                            NextWorkout = DateTime.Today.AddDays(1)
                        }
                    }
                },
                new Workout
                {
                    Name = "bbb",
                    Description = "bbbbbbb",
                    Creator = new IdentityUser
                    {
                        UserName = "user1"
                    },
                    WorkoutCycleType = WorkoutCycleType.EveryNDays,
                    WorkDays = new List<WorkDay>()
                    {
                        new WorkDay()
                        {
                            NextWorkout = DateTime.Today
                        },
                        new WorkDay()
                        {
                            NextWorkout = DateTime.Today.AddDays(1)
                        }
                    }
                }
            };

    }
}
