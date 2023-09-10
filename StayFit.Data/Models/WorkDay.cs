using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StayFit.Data.Models
{
    public class WorkDay
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public ICollection<Exercise> Exercises { get; init; } = new HashSet<Exercise>(); 
        [Required]
        public string WorkoutId { get; set; }    
        public Workout Workout { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }
        public ICollection<UserExerciseLog> UserExerciseLogs { get; init; }
            = new HashSet<UserExerciseLog>();
    }
}
