using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StayFit.Web.Models.Exercises
{
    public class AddExerciseFormModel
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string Equipment { get; init; }
        public string BodyPart { get; init; }
    }
}
