using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayFit.Data.Models
{
    public class User
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required]
        [MaxLength(20)]
        public string Username { get; init; }
        [Required]
        [MaxLength(20)]
        public string Password { get; init; }
        public string CurrentWorkoutId { get; init; }
        public Workout CurrentWorkout { get; init; }
        public ICollection<Workout> UploadedWorkouts { get; init; } = new HashSet<Workout>();
    }
}
