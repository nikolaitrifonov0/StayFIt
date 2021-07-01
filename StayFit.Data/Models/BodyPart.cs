using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayFit.Data.Models
{
    public class BodyPart
    {
        public int Id { get; init; }
        [Required]
        public string Name { get; set; }
        public ICollection<Exercise> Exercises { get; init; } = new HashSet<Exercise>();
    }
}
