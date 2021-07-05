using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayFit.Data.Models
{
    public class UserExerciseLog
    {
        [Required]
        public string UserId { get; init; }
        public User User { get; init; }
        [Required]
        public string ExerciseId { get; init; }
        public Exercise Exercise { get; init; }
        [Required]
        public string WorkDayId { get; init; }
        public WorkDay WorkDay { get; set; }

    }
}
