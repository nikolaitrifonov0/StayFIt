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
        public string Description { get; init; }
        [Required]
        public ICollection<WorkDay> WorkDays { get; init; } = new HashSet<WorkDay>();
        [Required]
        public WorkoutCycleType WorkoutCycleType { get; set; }
        public int? CycleDays { get; set; }
    }
}
