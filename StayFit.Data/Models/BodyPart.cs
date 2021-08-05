using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static StayFit.Data.DataConstants;

namespace StayFit.Data.Models
{
    public class BodyPart
    {
        public int Id { get; init; }
        [Required]
        [MaxLength(EquipmentNameMaxLength)]
        public string Name { get; set; }
        public ICollection<Exercise> Exercises { get; set; } = new HashSet<Exercise>();
    }
}
