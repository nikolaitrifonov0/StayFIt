using System.ComponentModel.DataAnnotations;

namespace StayFit.Data.Models
{
    public class UserExerciseLog
    {
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        [Required]
        public string ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        [Required]
        public string WorkDayId { get; set; }
        public WorkDay WorkDay { get; set; }

    }
}
