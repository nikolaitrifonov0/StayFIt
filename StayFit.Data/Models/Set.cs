using System.ComponentModel.DataAnnotations;

namespace StayFit.Data.Models
{
    public class Set
    {
        public int Id { get; init; }
        [Required]
        public int Repetitions { get; set; }
        [Required]
        public string UserExerciseLogId { get; set; }
        public UserExerciseLog UserExerciseLog { get; set; }
    }
}
