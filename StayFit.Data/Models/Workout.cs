using StayFit.Data.Models.Enums.Workout;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayFit.Data.Models
{
    public class Workout
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required]
        [MaxLength(50)]
        public string Name { get; init; }
        [Required]
        public string Description { get; init; }
        [Required]
        public string CreatorId { get; init; }
        public User Creator { get; init; }
        [Required]
        public ICollection<WorkDay> WorkDays { get; init; } = new HashSet<WorkDay>();
        [Required]
        public WorkoutCycleType WorkoutCycleType { get; init; }
        public int? CycleDays { get; set; }
        public ICollection<User> Users { get; init; } = new HashSet<User>();
    }
}
