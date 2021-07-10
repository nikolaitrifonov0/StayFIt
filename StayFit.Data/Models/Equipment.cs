using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static StayFit.Data.DataConstants;

namespace StayFit.Data.Models
{
    public class Equipment
    {
        public int Id { get; init; }
        [Required]
        [MaxLength(BodyPartNameMaxLength)]
        public string Name { get; set; }
        public ICollection<Exercise> Exercises { get; init; } = new HashSet<Exercise>();
    }
}
