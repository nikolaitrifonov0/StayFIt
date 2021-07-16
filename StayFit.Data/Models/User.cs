using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static StayFit.Data.DataConstants;

namespace StayFit.Data.Models
{
    public class User
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required]
        [MaxLength(UsernameMaxLength)]
        public string Username { get; set; }
        [Required]
        [MaxLength(PasswordMaxLength)]
        public string Password { get; set; }
        [Required]
        public string CurrentWorkoutId { get; set; }
        public Workout CurrentWorkout { get; set; }
        public ICollection<Workout> UploadedWorkouts { get; init; } = new HashSet<Workout>();
        public ICollection<UserExerciseLog> UserExerciseLogs { get; init; } 
            = new HashSet<UserExerciseLog>();
    }
}
