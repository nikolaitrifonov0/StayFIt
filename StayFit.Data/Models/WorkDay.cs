using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayFit.Data.Models
{
    public class WorkDay
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required]
        public DateTime NextWorkout { get; set; }
        [Required]
        public ICollection<Exercise> Exercises { get; init; } = new HashSet<Exercise>();
        public ICollection<Workout> Workouts { get; init; } = new HashSet<Workout>();
    }
}
