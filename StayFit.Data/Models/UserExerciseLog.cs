using System.ComponentModel.DataAnnotations;

namespace StayFit.Data.Models
{
    public class UserExerciseLog
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        public int Repetitions { get; set; }
        public int Sets { get; set; }
        [Required]
        public string WorkDayId { get; set; }
        public WorkDay WorkDay { get; set; }

    }
}
