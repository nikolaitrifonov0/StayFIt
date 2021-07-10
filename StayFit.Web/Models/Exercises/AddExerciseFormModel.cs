using System.Collections.Generic;

namespace StayFit.Web.Models.Exercises
{
    public class AddExerciseFormModel
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string Equipment { get; init; }
        public string BodyPart { get; init; }

        public IEnumerable<ExerciseEquipmentViewModel> Equipments { get; set; }
        public IEnumerable<ExerciseBodyPartViewModel> BodyParts { get; set; }
    }
}
