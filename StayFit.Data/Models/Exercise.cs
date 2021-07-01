using StayFit.Data.Models.Enums.Exercise;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayFit.Data.Models
{
    public class Exercise
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required]
        [MaxLength(50)]
        public string Name { get; init; }
        [Required]
        public string Description { get; init; }
        [Required]
        public Equipment Equipment { get; init; }
        [Required]
        public ICollection<BodyPart> BodyParts { get; init; } = new HashSet<BodyPart>();        
        public string ImageUrl { get; init; }
        public string VideoUrl { get; init; }
        public ICollection<WorkDay> WorkDays { get; init; } = new HashSet<WorkDay>();
    }
}
