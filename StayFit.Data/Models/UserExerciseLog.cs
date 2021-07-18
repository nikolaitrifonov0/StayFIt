using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StayFit.Data.Models
{
    public class UserExerciseLog
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required]
        public string UserId { get; set; }
        [Required]
        public string ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        public ICollection<Set> Sets { get; set; }
        [Required]
        public string WorkDayId { get; set; }
        public WorkDay WorkDay { get; set; }

    }
}
