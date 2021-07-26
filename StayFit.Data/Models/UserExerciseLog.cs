using System;
using System.ComponentModel.DataAnnotations;

using static StayFit.Data.DataConstants;

namespace StayFit.Data.Models
{
    public class UserExerciseLog
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required]
        [MaxLength(SetMaxLength)]
        public int SetNumber { get; set; }        
        public int Repetitions { get; set; }       
        [MaxLength(WeightMaxLenght)]
        public int? Weight { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string ExerciseId { get; set; }
        public Exercise Exercise { get; set; }        
        [Required]
        public string WorkDayId { get; set; }
        public WorkDay WorkDay { get; set; }
        public DateTime Date { get; set; }

    }
}
